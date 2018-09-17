using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using LuisBot.Controllers;
using BingMapsRESTToolkit;


namespace LuisBot.Dialogs
{
    [Serializable]
    public class GetAddressDialog : IDialog<string>
    {
        private int attempts = 3;

        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync("Where are you going:");
            context.Wait(this.MessageReceivedAsync);
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;
            if ((message.Text != null) && (message.Text.Trim().Length > 0))
            {
                Location sresult = await BingQuery.GetGeoLoc(message.Text);
                if (sresult != null)
                {
                    string imageurl = await BingQuery.GetImageURL(sresult);
                    HeroCard addcard = await CardController.AddressCard(sresult, imageurl);
                    Attachment adattach = addcard.ToAttachment();
                    var cmessage = context.MakeMessage();
                    cmessage.Attachments.Add(adattach);
                    await context.PostAsync(cmessage);
                    context.UserData.SetValue<Location>("SelectedLocation", sresult);
                    await context.PostAsync("Is this the correct location: Yes/No?");
                    context.Wait(this.ValidateAddressAsync);
                }
                else
                {
                    --attempts;
                    if (attempts > 0)
                    {
                        await context.PostAsync("Sorry that search returned zero results, please try an address, City and State, or Postal Code:");
                        context.Wait(this.MessageReceivedAsync);
                    }
                    else
                    {
                        await context.PostAsync("Sorry you have tried too many times, returning to main menu!");
                        context.Fail(new TooManyAttemptsException("Too many attempts with incorrect adddress"));
                    }
                }
            }
            else
            {
                --attempts;
                if (attempts > 0)
                {
                    await context.PostAsync("Sorry you must enter something to search, try and address or City and State or just a postal code:");
                    context.Wait(this.MessageReceivedAsync);
                }
                else
                {
                    await context.PostAsync("Sorry you have tried too many times, returning to main menu!");
                    context.Fail(new TooManyAttemptsException("Error blank input"));
                }
            }
        }

        private async Task ValidateAddressAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;
            if (message.Text.ToLower().StartsWith("y"))
            {
                context.Done("Stored User State");
            }
            else
            {
                context.UserData.RemoveValue("SelectedLocation");
                await context.PostAsync("Okay let's try again! What is the location you are interested in:");
                context.Wait(this.MessageReceivedAsync);
            }

        }
    }
}