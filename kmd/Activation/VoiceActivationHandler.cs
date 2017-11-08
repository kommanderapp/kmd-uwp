using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.Media.SpeechRecognition;

namespace kmd.Activation
{
    internal class VoiceActivationHandler : ActivationHandler<VoiceCommandActivatedEventArgs>
    {
        protected override Task HandleInternalAsync(VoiceCommandActivatedEventArgs args)
        {
            SpeechRecognitionResult speechRecognitionResult = args.Result;
            var command = speechRecognitionResult.RulePath[0];
            var fileType = SemanticInterpretation("fileType", speechRecognitionResult);
            var fileName = SemanticInterpretation("fileName", speechRecognitionResult);
            throw new ArgumentNullException();
        }

        private string SemanticInterpretation(string interpretationKey, SpeechRecognitionResult speechRecognitionResult)
        {
            return speechRecognitionResult.SemanticInterpretation.Properties[interpretationKey].FirstOrDefault();
        }
    }
}
