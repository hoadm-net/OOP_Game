using System;

namespace Lab04
{
    // Interface for characters that can use shield ability
    public interface IShieldable
    {
        // Interface properties
        bool IsShielding { get; }
        int ShieldDuration { get; }
        
        // Interface methods - must be implemented
        void ActivateShield();
        void DeactivateShield();
        bool CanUseShield();
    }
}