using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpeningTrainingCreator : MonoBehaviour
{
    public OpeningTrainingController openingTrainingController;
    
    public Text trainingsModeText;
    public InputField inputFieldDepth;
    private int currentDepth = 1;

    public SliderControllerUI depthSliderController;


    // TRAININGS MODE
    
    private int currentIndex = 0;
    private TrainingMode[] modes = {TrainingMode.Learning,TrainingMode.Randomized,TrainingMode.Testing};
    private string[] modeNames = {"Learning", "Randomized", "Testing"};
    public void StartOpeningTraining()
    {
        Opening opening = GameManager.instance.selcetedOpening;

        currentDepth = (int)depthSliderController.slider.value;
        switch (currentIndex)
        {
            case 0:
                openingTrainingController.InitTraining(opening, currentDepth, modes[0]);
                break;
            case 1:
                openingTrainingController.InitTraining(opening, currentDepth, modes[1]);
                break;
            case 2:
                openingTrainingController.InitTraining(opening, currentDepth, modes[2]);
                break;
        }
    }
    
    public void Init()
    {
        currentDepth = 3;
        depthSliderController.Init(0,3,currentDepth);
        depthSliderController.slider.value = 3;
    }
    public void GoNextMode()
    {
        if(currentIndex == 2)
        {
            currentIndex = 0;
        }
        else
        {  
            currentIndex++;
        }
        trainingsModeText.text = modeNames[currentIndex];
    }
    public void GoPreviousMode()
    {
        if(currentIndex == 0)
        {
            currentIndex = 2;
        }
        else
        {
            currentIndex--;
        }
        trainingsModeText.text = modeNames[currentIndex];
    }
}
