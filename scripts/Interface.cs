using Godot;
using System;

public class Interface : Control
{
    private Globals Globals;
    private Level Level;

    private Control _healthBar;
    private ColorRect _healthBarTR;
    private Label _scoreLabel;
    private Label _highScoreLabel;

    private AnimationPlayer _interfaceAnimation;

    private bool _healthBarShowed;

    private float _screenSize;
    private float _updateHealthBy;

    public override void _Ready()
    {
        Globals = GetNode<Globals>("/root/Globals");
        Level = GetNode<Level>("/root/Level");

        _interfaceAnimation = GetNode<AnimationPlayer>("InterfaceAnim");

        _healthBar = GetNode<Control>("Main/Health");
        _healthBarTR = GetNode<ColorRect>("Main/Health/Bar");

        _highScoreLabel = GetNode<Label>("Main/HighScore");
        _scoreLabel = GetNode<Label>("Main/Score");

        _GetScreenSize();
        _GetPreviousHighscore();
    }

    public void CalculateHealthBar()
    {
        if (!_healthBarShowed)
        {
            _interfaceAnimation.Play("healthbar_show");
            _healthBarShowed = true;
        }
        
        float health = Globals.playerHealth * _updateHealthBy;

        Vector2 pos = new(health, 16);
        _healthBar.SetSize(pos, false);
    }

    public void CalculatePerspectiveBar(float frames)
    {
        // Game physics runs 120f/s, 5s is 600f
        frames /= -Globals.FIVE_SEC_IN_FRAMES;
        // Reverse percentage
        frames += 1;

        float newSize = _screenSize * frames;

        Vector2 pos = new(newSize, 16);
        _healthBar.SetSize(pos, false);
    }

    public void UpdateScore()
    {
        _scoreLabel.Text = Globals.sessionScore.ToString();

        // Fix PlayOrthogonalAnimation() freezing
        if (!_interfaceAnimation.IsPlaying())
        {
            _interfaceAnimation.Play("score_bump");
        }
    }

    public void HideUiAnimations()
    {
        _interfaceAnimation.Play("ui_hide");
    }

    public void HideUiAnimationsHighscore()
    {
        _interfaceAnimation.Play("ui_hide_highscore");
    }

    public void UpdateHealthbarColor()
    {
        SpatialMaterial newHue3 = Globals.GetEmissionMaterial(0.25f);

        _healthBarTR.Color = newHue3.Emission;
    }

    public void PlayPerspectiveAnimation()
    {
        _interfaceAnimation.Play("blind_perspective");
    }

    public void PlayOrthogonalAnimation()
    {
        _interfaceAnimation.Play("blind_orthogonal");
    }

    public void ShowStatistics()
    {
        _interfaceAnimation.Play("stats_view_show");
    }

    public void HideStatistics()
    {
        _interfaceAnimation.Play("stats_view_hide");
    }

    private void _GetScreenSize()
    {
        _screenSize = GetViewport().Size.x;
        _updateHealthBy = _screenSize / Globals.FULL_HEALTH;
    }

    private void _GetPreviousHighscore()
    {
        Globals.highScore = Globals.LoadStats("HighScore");
        _highScoreLabel.Text = "HiScore: " + Globals.highScore;
    }

    private void _OnLeftButtonDown()
    {
        Level.CheckMove(Direction.LeftUp);
    }

    private void _OnRightButtonDown()
    {
        Level.CheckMove(Direction.RightDown);
    }

    private void _OnUpButtonDown()
    {
        Level.CheckMove(Direction.RightUp);
    }

    private void _OnDownButtonDown()
    {
        Level.CheckMove(Direction.LeftDown);
    }
}