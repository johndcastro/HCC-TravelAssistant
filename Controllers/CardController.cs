using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Bot;
using BingMapsRESTToolkit;
using Microsoft.Bot.Connector;
using System.Threading.Tasks;
using LuisBot.Objects;
using Microsoft.Bot.Builder.Dialogs;


namespace LuisBot.Controllers
{
   class CardController
    {
        public static async Task<HeroCard> AddressCard(Location inloc, string inurl)
        {

            List<CardImage> cardImages = new List<CardImage>();
            cardImages.Add(new CardImage(url: inurl));

            HeroCard newcard = new HeroCard()
            {
                Title = "Selected Location",
                Subtitle = inloc.Address.FormattedAddress,
                Images = cardImages
            };

            return newcard;
        }

        public static async Task AirportCards(List<airport> fports, IDialogContext context, Location storloc)
        {
            List<HeroCard> aircards = new List<HeroCard>();
            var cmessage = context.MakeMessage();
            foreach (airport aport in fports)
            {
                Location aportloc = await BingQuery.GetRevGeo(aport.longitude, aport.latitude);
                DistanceMatrix drivedist = await BingQuery.GetDriveDistance(storloc, aportloc);
                string imageurl = await BingQuery.GetImageURL(aportloc);
                List<CardImage> cardImages = new List<CardImage>();
                cardImages.Add(new CardImage(url: imageurl));
                HeroCard newcard = new HeroCard()
                {
                    Title = aport.name,
                    Subtitle = $"Driving Time: {Math.Ceiling(drivedist.GetTime(0, 0))} minutes",
                    Images = cardImages
                };
                Attachment airattach = newcard.ToAttachment();
                cmessage.Attachments.Add(airattach);
            }
            await context.PostAsync(cmessage);
        }


    }
}