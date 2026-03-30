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
        openingTrainingController.InitTraining(opening, currentDepth, modes[currentIndex]);
    }
    
    public void Init()
    {
        Opening opening = GameManager.instance.selcetedOpening;
        currentDepth = opening.GetMaxDepth(opening.rootNode);
        depthSliderController.Init(1,currentDepth,1);
        depthSliderController.slider.value = 1;
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
