using System;
using RoR2;

namespace ItemsHighlightConfigurator.Models.Exceptions;

public class DuplicateTierException(ItemTier tier) : Exception
{
    public override string Message => $"{tier} config already registered";
}