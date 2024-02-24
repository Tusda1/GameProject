using System.Collections;
using UnityEngine;
using TMPro;

public class TextArchitect
{
    private readonly TextMeshProUGUI _tmproUI;
    private readonly TextMeshPro _tmproWorld;
    public TMP_Text Tmpro => _tmproUI != null ? _tmproUI : _tmproWorld;

    public string TargetText { get; private set; } = "";
    public string PreText { get; private set; } = "";

    public string FullTargetText => PreText + TargetText;
    
    public enum BuildMethod { Instant, Typewriter, Fade}
    public BuildMethod buildMethod = BuildMethod.Typewriter;
    
    public Color TextColor
    {
        get => Tmpro.color;
        set => Tmpro.color = value;
    }
    
    private const float _speed = 0.6f;
    public int CharactersPerCycle =>
        _speed <= 2f ? _characterMultiplier :
        _speed <= 2.5f ? _characterMultiplier * 2 : _characterMultiplier * 3;

    private int _characterMultiplier = 1; 
    

    public TextArchitect(TextMeshProUGUI tmproUI)
    {
        this._tmproUI = tmproUI;
    }

    public TextArchitect(TextMeshPro tmproWorld)
    {
        this._tmproWorld = tmproWorld;
    }

    public Coroutine Build(string text)
    {
        PreText = "";
        TargetText = text;
        
        Stop();

        _buildProcess = Tmpro.StartCoroutine(Building());
        return _buildProcess;
    }
    
    public Coroutine Append(string text)
    {
        PreText = Tmpro.text;
        TargetText = text;
        
        Stop();

        _buildProcess = Tmpro.StartCoroutine(Building());
        return _buildProcess;
    }

    private Coroutine _buildProcess;
    public bool IsBuilding => _buildProcess != null;

    private void Stop()
    {
        if (!IsBuilding)
            return;
        
        Tmpro.StopCoroutine(_buildProcess);
        _buildProcess = null;
    }

    IEnumerator Building()
    {
        Prepare();
        switch (buildMethod)
        {
            case BuildMethod.Typewriter:
                yield return Build_Typewriter();
                break;
            case BuildMethod.Fade:
                yield return Build_Fade();
                break;
        }
        OnComplete();
    }

    private void OnComplete()
    {
        _buildProcess = null;
    }

    private void Prepare()
    {
        switch(buildMethod)
        {
            case BuildMethod.Instant:
                prepare_Instant();
                break;
            case BuildMethod.Typewriter:
                prepare_Typewriter();
                break;
            case BuildMethod.Fade:
                prepare_Fade();
                break;
        }
    }

    private void prepare_Instant()
    {
        Tmpro.color = Tmpro.color;
        Tmpro.text = FullTargetText;
        Tmpro.ForceMeshUpdate();
        Tmpro.maxVisibleCharacters = Tmpro.textInfo.characterCount;
    }

    private void prepare_Typewriter()
    {
        Tmpro.color = Tmpro.color;
        Tmpro.maxVisibleCharacters = 0;
        Tmpro.text = PreText;

        if (PreText != "")
        {
            Tmpro.ForceMeshUpdate();
            Tmpro.maxVisibleCharacters = Tmpro.textInfo.characterCount;
        }

        Tmpro.text += TargetText;
        Tmpro.ForceMeshUpdate();
    }

    private void prepare_Fade()
    {
        
    }

    private IEnumerator Build_Typewriter()
    {
        int counter = 0;
        
        while (Tmpro.maxVisibleCharacters < Tmpro.textInfo.characterCount)
        {
            Tmpro.maxVisibleCharacters += CharactersPerCycle;

            if (counter > 1)
            {
                GameEvents.playWritingSound?.Invoke();
                counter = 0;
            }
            else
            {
                counter++;
            }
            
            
            yield return new WaitForSeconds(0.015f / _speed);
        }
    }

    private IEnumerator Build_Fade()
    {
        yield return null;
    }
}
