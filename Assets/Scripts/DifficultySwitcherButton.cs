public class DifficultySwitcherButton : SwitcherButton
{

    public override void Start()
    {
        switch (SettingsContainer.difficulty)
        {
            case Difficulty.Easy:
                currentIndex = 0;
                button.text = options[0];
                break;
            case Difficulty.Normal:
                currentIndex = 1;
                button.text = options[1];
                break;
            case Difficulty.Tricky:
                currentIndex = 2;
                button.text = options[2];
                break;
        }
    }

    public override void Click()
    {
        base.Click();

        switch (this.currentIndex)
        {
            case 0:
                SettingsContainer.difficulty = Difficulty.Easy;
                break;
            case 1:
                SettingsContainer.difficulty = Difficulty.Normal;
                break;
            case 2:
                SettingsContainer.difficulty = Difficulty.Tricky;
                break;

        }
    }
}