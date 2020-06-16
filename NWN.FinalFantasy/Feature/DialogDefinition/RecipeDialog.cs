﻿using NWN.FinalFantasy.Enumeration;
using NWN.FinalFantasy.Service;
using NWN.FinalFantasy.Service.DialogService;
using static NWN.FinalFantasy.Core.NWScript.NWScript;
using Player = NWN.FinalFantasy.Core.NWNX.Player;

namespace NWN.FinalFantasy.Feature.DialogDefinition
{
    public class RecipeDialog : DialogBase
    {
        private class Model
        {
            public SkillType SelectedSkill { get; set; }
            public RecipeCategoryType SelectedCategory { get; set; }
            public RecipeType SelectedRecipe { get; set; }
            public bool IsFabricator { get; set; }
            public bool IsCrafting { get; set; }
        }

        private const string MainPageId = "MAIN_PAGE";
        private const string CategoryPageId = "CATEGORY_PAGE";
        private const string RecipeListPageId = "RECIPE_LIST_PAGE";
        private const string RecipePageId = "RECIPE_PAGE";

        public override PlayerDialog SetUp(uint player)
        {
            var model = new Model();

            // This menu can be entered one of two ways:
            //    1.) From the player's rest menu.
            //    2.) From using a crafting fabricator.
            // If the second event happens, we need to skip over the first page.
            var state = Craft.GetPlayerCraftingState(player);
            model.SelectedSkill = state.DeviceSkillType;

            if (model.SelectedSkill != SkillType.Invalid)
            {
                model.IsFabricator = true;
            }

            var builder = new DialogBuilder()
                .WithDataModel(model)
                .AddBackAction(Back)
                .AddEndAction(End);

            if (!model.IsFabricator)
                builder.AddPage(MainPageId, MainPageInit);

            builder.AddPage(CategoryPageId, CategoryPageInit)
                    .AddPage(RecipeListPageId, RecipeListPageInit)
                    .AddPage(RecipePageId, RecipePageInit);

            return builder.Build();
        }

        private void End()
        {
            var player = GetPC();
            var model = GetDataModel<Model>();

            if (model.IsFabricator)
            {
                AssignCommand(player, () => ActionInteractObject(OBJECT_SELF));
            }
            else
            {
                Craft.ClearPlayerCraftingState(player);
            }
        }

        private void Back(string oldPage, string newPage)
        {
            if (newPage == MainPageId)
            {
                var model = GetDataModel<Model>();
                model.SelectedCategory = RecipeCategoryType.Invalid;
                model.SelectedRecipe = RecipeType.Invalid;

                if (!model.IsFabricator)
                    model.SelectedSkill = SkillType.Invalid;
            }
        }

        private void MainPageInit(DialogPage page)
        {
            var model = GetDataModel<Model>();

            page.Header = "Please select a skill.";

            page.AddResponse(Skill.GetSkillDetails(SkillType.Blacksmithing).Name, () =>
            {
                model.SelectedSkill = SkillType.Blacksmithing;
                ChangePage(CategoryPageId);
            });

            page.AddResponse(Skill.GetSkillDetails(SkillType.Leathercraft).Name, () =>
            {
                model.SelectedSkill = SkillType.Leathercraft;
                ChangePage(CategoryPageId);
            });

            page.AddResponse(Skill.GetSkillDetails(SkillType.Alchemy).Name, () =>
            {
                model.SelectedSkill = SkillType.Alchemy;
                ChangePage(CategoryPageId);
            });

            page.AddResponse(Skill.GetSkillDetails(SkillType.Cooking).Name, () =>
            {
                model.SelectedSkill = SkillType.Cooking;
                ChangePage(CategoryPageId);
            });
        }


        private void CategoryPageInit(DialogPage page)
        {
            var model = GetDataModel<Model>();
            page.Header = "Please select a category.";

            foreach (var (key, value) in Craft.GetRecipeCategoriesBySkill(model.SelectedSkill))
            {
                page.AddResponse(value.Name, () =>
                {
                    model.SelectedCategory = key;
                    ChangePage(RecipeListPageId);
                });
            }
        }

        private void RecipeListPageInit(DialogPage page)
        {
            var model = GetDataModel<Model>();
            page.Header = "Please select a recipe.";

            foreach (var (key, value) in Craft.GetRecipesBySkillAndCategory(model.SelectedSkill, model.SelectedCategory))
            {
                page.AddResponse(value.Name, () =>
                {
                    model.SelectedRecipe = key;
                    ChangePage(RecipePageId);
                });
            }
        }

        private void RecipePageInit(DialogPage page)
        {
            var model = GetDataModel<Model>();
            var player = GetPC();
            var meetsRequirements = true;

            string BuildHeader()
            {
                var recipe = Craft.GetRecipe(model.SelectedRecipe);
                var category = Craft.GetCategoryDetail(recipe.Category);
                var skill = Skill.GetSkillDetails(recipe.Skill);

                // Recipe quantity and name.
                var header = $"{ColorToken.Green("Recipe:")} {recipe.Quantity}x {recipe.Name} \n";

                // Associated skill
                header += $"{ColorToken.Green("Craft:")} {skill.Name}\n";

                // Associated category
                header += $"{ColorToken.Green("Category:")} {category.Name}\n";

                // Recipe's description, if available.
                if (!string.IsNullOrWhiteSpace(recipe.Description))
                {
                    header += $"{ColorToken.Green("Description:")} {recipe.Description} \n";
                }

                // Chance to craft
                header += $"{ColorToken.Green("Chance to Auto-Craft:")} {Craft.CalculateChanceToCraft(player, model.SelectedRecipe)}%";

                // List of requirements, if applicable.
                if (recipe.Requirements.Count > 0)
                {
                    header += $"\n{ColorToken.Green("Requirements:")}\n\n";

                    foreach (var req in recipe.Requirements)
                    {
                        // If the player meets the requirement, show it in green. Otherwise show it in red.
                        if (string.IsNullOrWhiteSpace(req.CheckRequirements(player)))
                        {
                            header += $"{ColorToken.Green(req.RequirementText)}\n";
                        }
                        else
                        {
                            header += $"{ColorToken.Red(req.RequirementText)}\n";
                            meetsRequirements = false;
                        }
                    }
                }

                // List of components
                header += $"\n\n{ColorToken.Green("Components:")}\n\n";

                foreach (var (resref, quantity) in recipe.Components)
                {
                    var name = Cache.GetItemNameByResref(resref);
                    header += $"{quantity}x {name}\n";
                }

                return header;
            }

            page.Header = BuildHeader();

            if (model.IsFabricator && meetsRequirements)
            {
                page.AddResponse("Select Recipe", () =>
                {
                    var state = Craft.GetPlayerCraftingState(player);
                    state.SelectedRecipe = model.SelectedRecipe;

                    EndConversation();
                    Player.ForcePlaceableInventoryWindow(player, OBJECT_SELF);
                });
            }
        }
    }
}
