using System;
using System.Diagnostics;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

/// <summary>
/// <para>
/// A type that represents a value that may not be present.
/// </para>
/// 
/// <para>
/// This type is used to represent the result of operations that may
/// fail. It is a replacement for the use of <c>null</c> as a way to
/// signal that a value is not present.
/// </para>
/// </summary>
/// <typeparam name="T">
///     Type of the value that may be present.
/// </typeparam>
public abstract class Option<T> : IXmlSerializable
{
    /// <summary>
    /// An object representing the absence of a value of type <c>T</c>.
    /// </summary>
    public static readonly Option<T> None = new _None();

    /// <summary>
    /// Whether or not the object has a present value, i.e., it is not
    /// <c>None</c>.
    /// </summary>
    public virtual bool IsSome => false;

    /// <summary>
    /// Create an object with a present value of type <c>T</c>.
    /// </summary>
    /// <param name="value">
    ///     Value wrapped by the <c>Option</c> type.
    /// </param>
    /// <returns>
    ///     Object of type <c>Option&lt;T&gt;</c> with the given
    ///     present value.
    /// </returns>
    static public Option<T> Some(T value)
    {
        if (value == null)
        {
            Debug.Assert(value != null, "An Option should not contain a null value. Consider using a nested Option or a None Option instead");
            return new _None();
        }
        return new _Some(value);
    }

    /// <summary>
    /// <para>
    /// Return the value wrapped by the <c>Option</c> type, possibly
    /// raising an exception if the object is <c>None</c>.
    /// </para>
    /// 
    /// <para>
    /// This method can raise an exception, possibly interrupting the
    /// execution of the program. It is recommended to use other methods
    /// like <see cref="UnwrapOr"/> or <see cref="Match"/> to handle the
    /// absence of a value safely.
    /// </para>
    /// </summary>
    /// <returns>
    ///     The value wrapped by the <c>Option</c> type if the object is
    ///     <c>Some</c>.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    ///     Thrown when the object is <c>None</c>.
    /// </exception>
    abstract public T Unwrap();
    
    abstract public bool TryUnwrap(out T value);

    /// <summary>
    /// Returns the value wrapped by the <c>Option</c> type, or a
    /// a default value passed as argument if the object is <c>None</c>.
    /// </summary>
    /// <param name="defaultValue">
    ///     Default value to return if the object is <c>None</c>.
    /// </param>
    /// <returns></returns>
    virtual public T UnwrapOr(T defaultValue)
    {
        return defaultValue;
    }

    /// <summary>
    /// <para>
    /// Execute one of two functions depending on the presence of a
    /// value.
    /// </para>
    /// 
    /// <para>
    /// If the object is <c>Some</c>, the method calls the first
    /// function with the wrapped value as its argument. If the object
    /// is <c>None</c>, the method calls the second function with no
    /// arguments.
    /// </para>
    /// 
    /// <para>
    /// This method is useful for applying a transformation to the value
    /// while offering a way to handle the absence of a value.
    /// </para>
    /// 
    /// <para>
    /// This method only works with functions that return a value. For
    /// the same method, but for actions, see <see cref="ActionMatch"/>.
    /// </para>
    /// </summary>
    /// <typeparam name="S">
    ///     Output type of the transformation function
    /// </typeparam>
    /// <param name="some">
    ///     Transformation to apply to the value in case the object is
    ///     <c>Some</c>.
    /// </param>
    /// <param name="none">
    ///     Function to call in case the object is <c>None</c>.
    /// </param>
    /// <returns>
    ///     The result of the transformation function if the object is
    ///     <c>Some</c>, or the result of the <c>none</c> function if
    ///     the object is <c>None</c>.
    /// </returns>
    virtual public S Match<S>(Func<T, S> some, Func<S> none)
    {
        return none.Invoke();
    }

    /// <summary>
    /// <para>
    /// Execute one of two actions depending on whether the object is
    /// present or not.
    /// </para>
    /// 
    /// <para>
    /// This method is similar to <see cref="Match"/>, but it is used
    /// for actions that do not return a value. If the object is
    /// <c>Some</c>, the method calls the first action. If the object is
    /// <c>None</c>, the method calls the second action.
    /// </para>
    /// </summary>
    /// <param name="some">
    ///     Action to invoke if the object is <c>Some</c>.
    /// </param>
    /// <param name="none">
    ///     Action to invoke if the object is <c>None</c>.
    /// </param>
    virtual public void Match(Action<T> some, Action none)
    {
        none.Invoke();
    }

    /// <summary>
    /// <para>
    /// Execute an action only if the object is present.
    /// </para>
    /// 
    /// </summary>
    /// <param name="some">
    ///     Action to invoke if the object is <c>Some</c>.
    /// </param>
    public void Match(Action<T> some)
    {
        Match(some, () => { return; });
    }

    /// <summary>
    /// <para>
    /// Execute one of two actions depending on whether the object is
    /// present or not. If the object is present, the action is executed only
    /// if that object satisfies the filter.
    /// </para>
    /// </summary>
    public void MatchIf(Action<T> some, Predicate<T> filter, Action none)
    {
        Match(
            val =>
            {
                if (filter.Invoke(val))
                {
                    some.Invoke(val);
                }
            },
            none
        );
    }

    /// <summary>
    /// <para>
    /// Execute an action only if the object is present and it satisfies the
    /// filter.
    /// </para>
    /// 
    /// </summary>
    public void MatchIf(Action<T> some, Predicate<T> filter)
    {
        MatchIf(some, filter, () => { return; });
    }

    /// <summary>
    /// Apply a transformation to the value wrapped by the <c>Option</c>
    /// type, if it is present, handling the case that the object is
    /// <c>None</c> by returning a <c>None</c>.
    /// </summary>
    /// <typeparam name="S">
    ///     Output type of the transformation.
    /// </typeparam>
    /// <param name="transform">
    ///     Transformation to apply to the value.
    /// </param>
    /// <returns>
    ///     Either a <c>Some</c> with the result of the transformation,
    ///     or a <c>None</c> if the value is absent.
    /// </returns>
    public Option<S> Do<S>(Func<T, S> transform)
    {
        return Match(
            some: value => Option<S>.Some(transform(value)),
            none: () => Option<S>.None
        );
    }

    public S DoOr<S>(Func<T, S> transform, S defaultValue)
    {
        return Match(
            some: value => transform(value),
            none: () => defaultValue
        );
    }

    /// <summary>
    /// Checks if the value inside this optional is the same of the argument.
    /// If there is no value, false is returned.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public bool Is(T value)
    {
        return DoOr(x => x.Equals(value), false);
    }

    public XmlSchema GetSchema()
    {
        return null;
    }

    public void ReadXml(XmlReader reader)
    {
        Match(some => 
        {
            XmlSerializer serializer = new(typeof(T));
            some = (T)serializer.Deserialize(reader);
        });
    }

    public void WriteXml(XmlWriter writer)
    {
        Match(some => 
        {
            XmlSerializer serializer = new(typeof(T));
            serializer.Serialize(writer, some);
        });
    }


    private class _Some : Option<T>
    {
        T _Value { get; }

        public override bool IsSome => true;

        public _Some(T value)
        {
            _Value = value;
        }

        override public T Unwrap()
        {
            return _Value;
        }

        override public T UnwrapOr(T defaultValue)
        {
            return _Value;
        }

        override public S Match<S>(Func<T, S> some, Func<S> none)
        {
            return some.Invoke(_Value);
        }

        override public void Match(Action<T> some, Action none)
        {
            some.Invoke(_Value);
        }

        public override bool TryUnwrap(out T value)
        {
            value = _Value;
            return true;
        }

    }

    private class _None : Option<T>
    {
        public override bool TryUnwrap(out T value)
        {
            value = default;
            return false;
        }


        override public T Unwrap()
        {
            throw new InvalidOperationException("Cannot unwrap a None value");
        }
    }
}