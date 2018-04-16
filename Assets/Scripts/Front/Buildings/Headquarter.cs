using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class Headquarter : Building
{
    private Unit unit;

    public override bool ContainsUnit() {
        if(unit != null) {
            return true;
        }
        return false;
    }

    public override bool PlaceUnit(Unit placement) {
        if (placement.HideInBuilding()) {
            unit = placement;
            return true;
        }
        return false;
    }

    public override bool RemoveUnit() {
        if(unit != null) {
            unit = null;
            return true;
        }
        return false;
    }
}
