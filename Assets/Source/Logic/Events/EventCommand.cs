using System;

namespace Source.Logic.Events
{
    public abstract class EventCommand
    {
        protected string ID => id;
        private string id;

        protected EventCommand()
        {
            id = Convert.ToBase64String(Guid.NewGuid().ToByteArray()).Substring(0, 4);
        }
        
        public abstract bool Perform();
    }
}
