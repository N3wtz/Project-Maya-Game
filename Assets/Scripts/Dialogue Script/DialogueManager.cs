using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    [Header("UI References")]
    public CanvasGroup canvasGroup;
    public TMP_Text actorName;
    public TMP_Text dialogueText;
    public Button[] choiceButtons;

    public bool isDialogueActive;

    private DialogueSO currentDialogue;
    private int dialogueIndex;

    private Coroutine typingCoroutine;
    private bool isTyping = false;
    private bool skipTyping = false;

    [Header("Typing Settings")]
    [Tooltip("Waktu delay per karakter untuk efek ketik")]
    public float typingSpeed = 0.05f;

    [Header("Cooldown Settings")]
    [Tooltip("Cooldown setelah dialog selesai sebelum bisa memulai lagi")]
    public float cooldownAfterDialogue = 1f;
    private bool isInCooldown = false;

    // Flag untuk mencegah input lanjut dialog terlalu cepat
    private bool canAdvance = true;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        foreach (var button in choiceButtons)
            button.gameObject.SetActive(false);
    }

    public void StartDialogue(DialogueSO dialogueSO)
    {
        if (isInCooldown) return;

        currentDialogue = dialogueSO;
        dialogueIndex = 0;
        isDialogueActive = true;
        ShowDialogue();
    }

    public void AdvanceDialogue()
    {
        if (!canAdvance) return;

        if (isTyping)
        {
            skipTyping = true;
        }
        else if (dialogueIndex < currentDialogue.lines.Length)
        {
            StartCoroutine(AdvanceWithDelay());
        }
        else
        {
            ShowChoices();
        }
    }

    private IEnumerator AdvanceWithDelay()
    {
        canAdvance = false;

        ShowDialogue();

        yield return new WaitForSeconds(typingSpeed * 2f);

        canAdvance = true;
    }

    private void ShowDialogue()
    {
        DialogueLine line = currentDialogue.lines[dialogueIndex];

        DialogueHistoryTracker.Instance.RecordNPC(line.speaker);

        actorName.text = line.speaker.actorName;

        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
        }

        typingCoroutine = StartCoroutine(TypeLine(line.text));

        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;

        dialogueIndex++;
    }

    private IEnumerator TypeLine(string text)
    {
        isTyping = true;
        skipTyping = false;
        dialogueText.text = "";

        foreach (char c in text)
        {
            if (skipTyping)
            {
                dialogueText.text = text;
                break;
            }

            dialogueText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
        skipTyping = false;
        typingCoroutine = null;
    }

    private void ShowChoices()
    {
        ClearChoices();

        if (currentDialogue.options.Length > 0)
        {
            for (int i = 0; i < currentDialogue.options.Length; i++)
            {
                var option = currentDialogue.options[i];
                var capturedOption = option; // fix closure issue

                choiceButtons[i].GetComponentInChildren<TMP_Text>().text = capturedOption.optionText;
                choiceButtons[i].gameObject.SetActive(true);

                choiceButtons[i].onClick.AddListener(() => ChooseOption(capturedOption.nextDialogue));
            }
        }
        else
        {
            choiceButtons[0].GetComponentInChildren<TMP_Text>().text = "End";
            choiceButtons[0].onClick.AddListener(EndDialogue);
            choiceButtons[0].gameObject.SetActive(true);
        }
    }

    private void ChooseOption(DialogueSO dialogueSO)
    {
        ClearChoices();
        isDialogueActive = false; // FIX: agar StartDialogue tidak diblok

        if (dialogueSO == null)
        {
            EndDialogue();
        }
        else
        {
            StartDialogue(dialogueSO);
        }
    }

    private void EndDialogue()
    {
        dialogueIndex = 0;
        isDialogueActive = false;
        ClearChoices();

        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        StartCoroutine(CooldownRoutine());
    }

    private void ClearChoices()
    {
        foreach (var button in choiceButtons)
        {
            button.gameObject.SetActive(false);
            button.onClick.RemoveAllListeners();
        }
    }

    private IEnumerator CooldownRoutine()
    {
        isInCooldown = true;
        canAdvance = false;
        yield return new WaitForSeconds(cooldownAfterDialogue);
        isInCooldown = false;
        canAdvance = true;
    }
}
