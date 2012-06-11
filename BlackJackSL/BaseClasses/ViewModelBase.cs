using System;
using System.ComponentModel;
using System.Linq.Expressions;

namespace BlackJackSL.BaseClasses
{
    public static class PropertyExtensions
    {
        public static PropertyChangedEventArgs CreateChangeEventArgs<T>(this Expression<Func<T>> property)
        {
            var expression = property.Body as MemberExpression;
            var member = expression.Member;
            return new PropertyChangedEventArgs(member.Name);
        }
    }

    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        public virtual event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        protected void RaisePropertyChanged<T>(Expression<Func<T>> property)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
                handler(this, property.CreateChangeEventArgs());
        }
    }
}