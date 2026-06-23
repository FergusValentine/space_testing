using System.Collections.Generic;

public interface IControllable
{
    public ActionMap actionMap { get; }

    void InputEnabled();
    void InputDisabled();
}