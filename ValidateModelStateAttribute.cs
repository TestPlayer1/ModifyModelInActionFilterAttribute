public class ValidateModelStateAttribute : ActionFilterAttribute
    {

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (!actionContext.ModelState.IsValid)
            {
                var apiParams = actionContext.ActionDescriptor.GetParameters();

                string rawRequest;
                using (var stream = new StreamReader(actionContext.Request.Content.ReadAsStreamAsync().Result))
                {
                    stream.BaseStream.Position = 0;
                    rawRequest = stream.ReadToEnd();
                }
           
                //Cast XML string to Object
                <your_class> newModel = FromXml<your_class>(rawRequest);
                                
                //Do your modification here using rawRequest.     
                //...
                
                //Set new model
                actionContext.ActionArguments["request"] = newModel;
                base.OnActionExecuting(actionContext);
            }
        }

        public static T FromXml<T>(string Xml) where T : new()
        {
            if (string.IsNullOrEmpty(Xml))
            {
                return new T();
            }

            XmlSerializer ser;
            ser = new XmlSerializer(typeof(T));
            StringReader stringReader;
            stringReader = new StringReader(Xml);
            XmlTextReader xmlReader;
            xmlReader = new XmlTextReader(stringReader);
            object obj;
            obj = ser.Deserialize(xmlReader);
            xmlReader.Close();
            stringReader.Close();
            return (T)obj;

        }
    }
