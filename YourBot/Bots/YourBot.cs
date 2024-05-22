// Generated with YourBot .NET Template version v4.22.0

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Azure;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using Azure.AI.OpenAI;
using System;

namespace YourBot.Bots
{

    public class YourBot : ActivityHandler
    {
        //Generar 2 propiedades para la llave y el endpoint de Azure OpenAI, con un valor predeterminado 
        private string openAiKey = "0b5e335c2cfb410cb7f4b1aad3cad525";
        private string openAiEndpoint = "https://aoai-dev-gedrm-aoaighcopilotws.openai.azure.com/";
        //Generar un método asíncrono que regrese un string y que reciba como parámetro un texto, el método esta vacío por el momento
        private async Task<string> GetOpenAiResponseAsync(string text)
        {
            //generar un AzureKeyCredential con la llave de OpenAI
            var credential = new AzureKeyCredential(openAiKey);
               //inicializar el cliente de OpenAIClient con el endpoint de OpenAI y el AzureKeyCredential
            var client = new OpenAIClient(new Uri(openAiEndpoint), credential );
            //Inicializar las opciones de ChatCompletionsOptions de forma inline indicando el parámetro de DeploymentName de tipo gpt-35-turbo y Messages recibido de la variable text de tipo ChatRequestUserMessage. Siguiendo el formato new Class { Property = Value }
            var options = new ChatCompletionsOptions { DeploymentName = "gpt-35-turbo", Messages = { new ChatRequestUserMessage(text) }};
            //Invocar el método GetChatCompletionsAsync y regresar la respuesta de la llamada
            var response = await client.GetChatCompletionsAsync(options);
            //regresar el texto de la respuesta de Choices en la variable response
            return response.Value.Choices[0].Message.Content;
            
        }
        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            //Declarar una variable de tipo string llamada replyText y asignarle el valor de la respuesta de GetOpenAIResponseAsync con el parámetro de texto de la actividad recibida
            var replyText = await GetOpenAiResponseAsync(turnContext.Activity.Text);
            //Crear un mensaje de texto con el texto de la variable replyText
            var reply = MessageFactory.Text(replyText);
            //Enviar el mensaje de respuesta
            await turnContext.SendActivityAsync(reply, cancellationToken);

            
        }

        protected override async Task OnMembersAddedAsync(IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            var welcomeText = "Hello and welcome!";
            foreach (var member in membersAdded)
            {
                if (member.Id != turnContext.Activity.Recipient.Id)
                {
                    await turnContext.SendActivityAsync(MessageFactory.Text(welcomeText, welcomeText), cancellationToken);
                }
            }
        }
    }
}
