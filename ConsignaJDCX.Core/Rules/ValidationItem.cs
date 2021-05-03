using System;

namespace ConsignaJDCX.Core.Rules
{
    public class ValidationItem<T>
    {
        public Predicate<T> Predicate { get; set; }
        public string MessageError { get; set; }
        public ValidationItem(Predicate<T> predicate, string messageError)
        {
            Predicate = predicate;
            MessageError = messageError;
        }
    }
}
