interface IInteracteable
{
    bool GetInteracted(InteractionsBehaviour target);

    string PromptText { get; }
}
