namespace BLL.Versions.V1.Builders
{
    public class JsonMessageResponseBuilder
    {
        private JsonMessageResponse response = new JsonMessageResponse();
        public JsonMessageResponse Build() => response;

        public JsonMessageResponseBuilder WithMessage(string msg)
        {
            response.Message = msg;
            return this;
        }
        public JsonMessageResponseBuilder WithMessageInfo(string msgInfo)
        {
            response.MessageInfo = msgInfo;
            return this;
        }
        public JsonMessageResponseBuilder WithEvent(string eventName)
        {
            response.Event = eventName;
            return this;
        }
        public JsonMessageResponseBuilder WithIsSuccess(bool isSuccess)
        {
            response.IsSuccess = isSuccess;
            return this;
        }

    }
}
