using System;

public class Flag
{
    private bool value;
    public Action setEvent = delegate { }; // better Speed etc, but not useful for in-Editor Events

    /// <summary>
    /// Standard Constructor with value set false; events initialized
    /// </summary>
    public Flag()
    {
        value = false;
        //setEvent = new();
    }

    /// <summary>
    /// Constructor with Event init and start value?
    /// </summary>
    /// <param name="val">Starting value</param>
    public Flag(bool val)
    {
        value = val;
        //setEvent = new();
    }


    /// <summary>
    /// Set()s Flags.value=true and calls registered setEvents()
    /// </summary>
    /// <returns>Return previous value</returns>
    public bool Set()
    {
        if (setEvent is not null)
            setEvent();

        if (value == true)
            return true;
        else
        {
            value = true;
            return false;
        }
    }

    /// <summary>
    /// Set()s conditionally, If (condition) is true
    /// </summary>
    /// <param name="condition">Put your Conditional Expression here</param>
    public void If(bool condition)
    {
        if (condition)
            Set();
    }


    /// <summary>
    /// Sets Flags.value false and calls registered resetEvents()
    /// </summary>
    /// <returns>Return previous value</returns>
    public bool Reset()
    {
        if (value == false)
            return false;
        else
        {
            value = false;
            return true;
        }
    }

    /// <summary>
    /// returns Flags.value
    /// </summary>
    /// <returns>value</returns>
    public bool Get()
    {
        return value;
    }

    /// <summary>
    /// Assigns bool to Flags.value and calls the appropriate event
    /// </summary>
    /// <param name="val">New value</param>
    public void Put(bool val)
    {
        if (val)
            Set();
        else
            Reset();
    }

    /// <summary>
    /// Assigns bool to Flags.value WITHOUT calling either event
    /// </summary>
    /// <param name="val">New value</param>
    public void PutSilent(bool val)
    {
        this.value = val;
    }



    #region Operator overrides, very useful for reding value:

    /// <summary>
    /// Implicit [=] and explicit [(bool)] conversion from Flag to bool -> returns our value as bool
    /// </summary>
    /// <param name="f">Flag</param>
    public static implicit operator bool(Flag f) => f.value;    // hand out value for all boolean operations with Flag-object
                                                                // alias implizierte Konvertierung Flag -> bool

    /// <summary>
    /// !!!CONVERTS bool into NEW Flag, might be useful for creating new Flag (that's why only explicit)
    /// </summary>
    /// <param name="val">new Flag from this</param>
    public static explicit operator Flag(bool val) => new Flag(val);

    #region Uneccessary
    //public static explicit operator bool(Flag f) => f.value;  // explizite konvertrierung: nur {bool b = (bool)f;} funktioniert
    //public static bool operator true(Flag f) => f.value;      // hand out only for if(Flag) {}
    //public static bool operator false(Flag f) => f.value;     // hand out only for if(!Flag) {}

    // These would implicitly create a NEW Flag from a bool, not what we need
    /*
    /// <summary>
    /// Implicit assignment of Flag.value
    /// </summary>
    /// <param name="val"></param>
    public static implicit operator Flag(bool val)
    {
        return new Flag(false, val);
    }
    */
    //public static implicit operator Flag(bool val) => new Flag(false, val);
    #endregion

    /// <summary>
    /// Shorthand for Flag.Set()
    /// </summary>
    /// <param name="f">Flag</param>
    /// <returns>self after set</returns>
    public static Flag operator ++(Flag f)
    {
        f.Set();
        return f;
    }

    /// <summary>
    /// Shorthand for Flag.Reset()
    /// </summary>
    /// <param name="f">Flag</param>
    /// <returns>self after reset</returns>
    public static Flag operator --(Flag f)
    {
        f.Reset();
        return f;
    }

    #endregion
}
