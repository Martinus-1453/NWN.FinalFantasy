using NWN.FinalFantasy.Entity;
using NWN.FinalFantasy.Service;
using NWN.FinalFantasy.Service.DialogService;
using NWN.FinalFantasy.Service.TripleTriadService;
using static NWN.FinalFantasy.Core.NWScript.NWScript;

namespace NWN.FinalFantasy.Feature.DialogDefinition
{
    public class CardDialog: DialogBase
    {
        private class Model
        {
            public CardType SelectedCardType { get; set; }
        }

        private const string MainPageId = "MAIN_PAGE";
        private const string ViewCardsPageId = "VIEW_CARDS_PAGE";
        private const string ViewCardDetailsPageId = "VIEW_CARD_DETAILS_PAGE";
        private const string ManageDecksPageId = "MANAGE_DECKS_PAGE";

        public override PlayerDialog SetUp(uint player)
        {
            var builder = new DialogBuilder()
                .WithDataModel(new Model())
                .AddPage(MainPageId, MainPageInit)
                .AddPage(ViewCardsPageId, ViewCardsPageInit)
                .AddPage(ViewCardDetailsPageId, ViewCardDetailsPageInit)
                .AddPage(ManageDecksPageId, ManageDecksPageInit);

            return builder.Build();
        }

        private void MainPageInit(DialogPage page)
        {
            page.Header = ColorToken.Green("Triple Triad Menu");

            page.AddResponse("View Cards", () => ChangePage(ViewCardsPageId));
            page.AddResponse("Manage Decks", () => ChangePage(ManageDecksPageId));
        }

        private void ViewCardsPageInit(DialogPage page)
        {
            var player = GetPC();
            var playerId = GetObjectUUID(player);
            var dbPlayerTripleTriad = DB.Get<PlayerTripleTriad>(playerId);
            var availableCards = TripleTriad.GetAllAvailableCards();
            var model = GetDataModel<Model>();

            page.Header = $"The following is the full list of cards available. Cards in {ColorToken.Green("GREEN")} have been acquired. Those in {ColorToken.Red("RED")} have not. Only one card per type can be collected.\n\n" +
                "Please select a card.";

            foreach (var card in availableCards)
            {
                if (!card.Value.IsVisibleInMenu) continue;

                var option = dbPlayerTripleTriad.AvailableCards.ContainsKey(card.Key) 
                    ? ColorToken.Green(card.Value.Name) 
                    : ColorToken.Red(card.Value.Name);

                page.AddResponse(option, () =>
                {
                    model.SelectedCardType = card.Key;
                    ChangePage(ViewCardDetailsPageId);
                });
            }

        }

        private void ViewCardDetailsPageInit(DialogPage page)
        {
            var player = GetPC();
            var playerId = GetObjectUUID(player);
            var dbPlayerTripleTriad = DB.Get<PlayerTripleTriad>(playerId);
            var model = GetDataModel<Model>();
            var card = TripleTriad.GetCardByType(model.SelectedCardType);
            var dateAcquired = dbPlayerTripleTriad.AvailableCards.ContainsKey(model.SelectedCardType)
                ? dbPlayerTripleTriad.AvailableCards[model.SelectedCardType].ToString("yyyy-MM-dd hh:mm:ss")
                : ColorToken.Red("Unacquired");

            page.Header = $"{ColorToken.Green("Name: ")} {card.Name}\n" +
                          $"{ColorToken.Green("Level: ")} {card.Level}\n" +
                          $"{ColorToken.Green("Date Acquired: ")} {dateAcquired}";

            // todo: Display a PostString version of the card, power, and element
        }

        private void ManageDecksPageInit(DialogPage page)
        {

        }

    }
}
