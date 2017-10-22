using UnityEngine;
using System.Linq;

public class Event {
    public Event() { }
    public string text = "";

    public virtual int probability { get; }
    public virtual bool isDone() { return true; }

    public virtual bool Generate() { return false; }
}

public class SliderEvent : Event {
    public override int probability { get { return 10; } }
    public Slider slider;
    public int goal;

    public override bool Generate() {
        Slider[] sliders = Object.FindObjectsOfType<Slider>();
        sliders = sliders.Where(s => { return s.enableEvent; }).ToArray();
        if (sliders.Length == 0) return false;

        int choice = Random.Range(0, sliders.Length);
        slider = sliders[choice];
        do {
            goal = Random.Range(0, slider.steps);
        } while (goal == slider.output);

        text = (goal > slider.output ? "Increase " : "Decrease ") + slider.eventLabel + " to " + goal + "!";
        return true;
    }

    public override bool isDone() {
        return slider.output == goal;
    }

    public SliderEvent() { }
}

public class ButtonEvent : Event {
    public override int probability { get { return 10; } }

    public Button button;
    public bool state;

    public override bool Generate() {
        Button[] buttons = Object.FindObjectsOfType<Button>();
        buttons = buttons.Where(s => { return s.enableEvent; }).ToArray();
        if (buttons.Length == 0) return false;

        int choice = Random.Range(0, buttons.Length);
        button = buttons[choice];
        state = !button.state;

        text = (choice > 4 ? "Activate " : "Press ") + button.eventLabel + "!";
        return true;
    }


    public override bool isDone() {
        return button.state == state;
    }

    public ButtonEvent() { }
}

public class DialEvent : Event {
    public override int probability { get { return 0; } }
    public Dial dial;
    public int goal;

    public override bool Generate() {
        Dial[] dials = Object.FindObjectsOfType<Dial>();
        dials = dials.Where(s => { return s.enableEvent; }).ToArray();
        if (dials.Length == 0) return false;

        int choice = Random.Range(0, dials.Length);
        dial = dials[choice];
        do {
            goal = Random.Range(0, dial.steps);
        } while (goal == dial.output);

        text = (goal > dial.output ? "Increase " : "Decrease ") + dial.eventLabel + " to " + goal + "!";
        return true;
    }

    public override bool isDone() {
        return dial.output == goal;
    }

    public DialEvent() { }
}

public class LeverEvent : Event {
    public override int probability { get { return 3; } }
    public Lever lever;
    public int goal;

    public override bool Generate() {
        Lever[] levers = Object.FindObjectsOfType<Lever>();
        levers = levers.Where(s => { return s.enableEvent; }).ToArray();
        if (levers.Length == 0) return false;

        int choice = Random.Range(0, levers.Length);
        lever = levers[choice];
        do {
            goal = Random.Range(0, lever.steps);
        } while (goal == lever.output);

        text = (goal > lever.output ? "Increase " : "Decrease ") + lever.eventLabel + " to " + goal + "!";
        return true;
    }

    public override bool isDone() {
        return lever.output == goal;
    }

    public LeverEvent() { }
}

public class PanelKickEvent : Event {
    public override int probability { get { return 7; } }
    public PanelKick panel;
    public int goal;

    public override bool Generate() {
        PanelKick[] panels = Object.FindObjectsOfType<PanelKick>();
        panels = panels.Where(s => { return s.enableEvent; }).ToArray();
        if (panels.Length == 0) return false;

        int choice = Random.Range(0, panels.Length);
        panel = panels[choice];
        panel.Activate();

        text = "Stomp the panel back into place!";
        return true;
    }

    public override bool isDone() {
        return !panel.isOpen;
    }

    public PanelKickEvent() { }
}

public class FireEvent : Event {
    public override int probability { get { return 4; } }
    public Fire fire;
    public int goal;

    public override bool Generate() {
        Fire[] fires = Object.FindObjectsOfType<Fire>();
        fires = fires.Where(s => { return s.enableEvent; }).ToArray();
        if (fires.Length == 0) return false;

        int choice = Random.Range(0, fires.Length);
        fire = fires[choice];
        fire.EventActivate();

        text = "Put out the fire in " + fire.eventLabel;
        return true;
    }

    public override bool isDone() {
        return !fire.isRaging;
    }

    public FireEvent() { }
}
