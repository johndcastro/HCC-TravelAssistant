using System;
using System.Configuration;
using System.Threading;
using System.Threading.Tasks;
using LuisBot.Dialogs;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using LuisBot.Controllers;
using BingMapsRESTToolkit;
using Microsoft.Bot.Connector;
using System.Collections.Generic;
using LuisBot.Objects;
using System.Xml.Linq;

namespace Microsoft.Bot.Sample.LuisBot
{
    // For more information about this template visit http://aka.ms/azurebots-csharp-luis
    [Serializable]
    public class BasicLuisDialog : LuisDialog<object>
    {
        public BasicLuisDialog() : base(new LuisService(new LuisModelAttribute(
            ConfigurationManager.AppSettings["LuisAppId"], 
            ConfigurationManager.AppSettings["LuisAPIKey"], 
            domain: ConfigurationManager.AppSettings["LuisAPIHostName"])))
        {
        }

        [LuisIntent("Greeting")]
        public async Task GreetingIntent(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Welcome to the bot, you can try things like Get the Per Diem for a location!");
            context.Wait(MessageReceived);
        }

        [LuisIntent("Cancel")]
        public async Task CancelIntent(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Thank you for trying the travel assistant!");
            context.UserData.RemoveValue("SelectedLocation");
            context.Wait(MessageReceived);

        }

        [LuisIntent("")]
        [LuisIntent("None")]
        [LuisIntent("Help")]
        public async Task HelpIntent(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("You can try one of the following items:");
            await context.PostAsync("Find the per diem");
            await context.PostAsync("Find the nearest airport");
            await context.PostAsync("Change my location");
            context.Wait(MessageReceived);
        }

        [LuisIntent("Airport")]
        public async Task AirportIntent(IDialogContext context, LuisResult result)
        {
            Location storloc = new Location();
            context.UserData.TryGetValue<Location>("SelectedLocation", out storloc);
            if (storloc == null)
            {
                context.Call(new GetAddressDialog(), this.AirportResumeAfter);
            }
            else
            {
               await AirportRange(context, storloc);

                context.Wait(MessageReceived);

            }
        }

        private async Task AirportResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            try
            {
                Location storloc = new Location();
                context.UserData.TryGetValue<Location>("SelectedLocation", out storloc);
                if (storloc == null)
                {
                    await context.PostAsync("I'm having issues with your location, lets start over!  What can I help you with today?");
                    context.Wait(MessageReceived);
                }
                else
                {
                    await AirportRange(context, storloc);

                    context.Wait(MessageReceived);
                }
            }
            catch (TooManyAttemptsException)
            {
                await context.PostAsync("I'm having issues understanding what you are saying, lets start over, What can I help you with today?");
                context.Wait(MessageReceived);
            }
        }

        private async Task AirportRange(IDialogContext context, Location storloc)
        {
            string imageurl = await BingQuery.GetImageURL(storloc);
            HeroCard addcard = await CardController.AddressCard(storloc, imageurl);
            Attachment adattach = addcard.ToAttachment();
            var cmessage = context.MakeMessage();
            cmessage.Attachments.Add(adattach);
            await context.PostAsync(cmessage);
            List<airport> fports = await AirportFind.GetAirportsRange(storloc.Point.GetCoordinate().Latitude, storloc.Point.GetCoordinate().Longitude, 100);
            if (fports.Count > 0)
            {
                await context.PostAsync($"I found {fports.Count} airports: ");
                await CardController.AirportCards(fports, context, storloc);
            }
            else
            {
                List<airport> fportsext = await AirportFind.GetAirportsRange(storloc.Point.GetCoordinate().Latitude, storloc.Point.GetCoordinate().Longitude, 200);
                if (fportsext.Count > 0)
                {
                    await context.PostAsync($"I found {fportsext.Count} airports: ");
                    await CardController.AirportCards(fportsext, context, storloc);
                }
                else
                {
                    await context.PostAsync("Sorry I couldn't find an airport within 200 miles");
                }
            }

        }


        [LuisIntent("PerDiem")]
        public async Task PerDiemIntent(IDialogContext context, LuisResult result)
        {
            Location storloc = new Location();
            context.UserData.TryGetValue<Location>("SelectedLocation", out storloc);
            if (storloc == null)
            {
                context.Call(new GetAddressDialog(), this.PerDiemResumeAfter);
            }
            else
            {
                string imageurl = await BingQuery.GetImageURL(storloc);
                HeroCard addcard = await CardController.AddressCard(storloc, imageurl);
                Attachment adattach = addcard.ToAttachment();
                var cmessage = context.MakeMessage();
                cmessage.Attachments.Add(adattach);
                await context.PostAsync(cmessage);
                Double mealrate = await PQuery.GetMealLoc(storloc.Address);
                string drate = String.Format("{0:C}", Convert.ToInt32(mealrate));
                string dtrate = String.Format("{0:C}", Convert.ToInt32((mealrate *.75)));
                await context.PostAsync($"Daily meal rate: Full Day - {drate} and Travel Day - {dtrate}");
                context.Wait(MessageReceived);

            }
        }

        private async Task PerDiemResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            try
            {
                Location storloc = new Location();
                context.UserData.TryGetValue<Location>("SelectedLocation", out storloc);
                if (storloc == null)
                {
                    await context.PostAsync("I'm having issues with your location, lets start over!  What can I help you with today?");
                    context.Wait(MessageReceived);
                }
                else
                {
                    string imageurl = await BingQuery.GetImageURL(storloc);
                    HeroCard addcard = await CardController.AddressCard(storloc, imageurl);
                    Attachment adattach = addcard.ToAttachment();
                    var cmessage = context.MakeMessage();
                    cmessage.Attachments.Add(adattach);
                    await context.PostAsync(cmessage);
                    Double mealrate = await PQuery.GetMealLoc(storloc.Address);
                    string drate = String.Format("{0:C}", Convert.ToInt32(mealrate));
                    await context.PostAsync($"Daily meal rate: Full Day - {drate} and Travel Day - {dtrate}");
                    context.Wait(MessageReceived);

                }
            }
            catch (TooManyAttemptsException)
            {
                await context.PostAsync("I'm having issues understanding what you are saying, lets start over, What can I help you with today?");
                context.Wait(MessageReceived);
            }
        }

        [LuisIntent("ChangeLocation")]
        public async Task ChangeLocationIntent(IDialogContext context, LuisResult result)
        {
            Location storloc = new Location();
            context.UserData.TryGetValue<Location>("SelectedLocation", out storloc);
            if (storloc == null)
            {
                context.Call(new GetAddressDialog(), this.LocationResumeAfter);
            }
            else
            {
                context.UserData.RemoveValue("SelectedLocation");
                context.Call(new GetAddressDialog(), this.LocationResumeAfter);
            }
        }

        private async Task LocationResumeAfter(IDialogContext context, IAwaitable<string> result)
        {
            await context.PostAsync("Okay location has been updated, what else can I help you with?");
            context.Wait(MessageReceived);
        }

        private async Task ShowLuisResult(IDialogContext context, LuisResult result) 
        {
            await context.PostAsync($"You have reached {result.Intents[0].Intent}. You said: {result.Query}");
            context.Wait(MessageReceived);
        }
    }
}