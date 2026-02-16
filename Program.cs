<<<<<<< HEAD
Console.WriteLine("================================================");
Console.WriteLine("    MANALILI - BIO-SAFE WATER TREATMENT");
Console.WriteLine("    Scenario D: The Clean-Water Plant");
Console.WriteLine("================================================\n");

try
{
    Console.WriteLine("[DEMO 1: Normal Filter Operation]");
    Console.WriteLine("----------------------------------------");
    
    CarbonFilter carbonFilter = new CarbonFilter("CARB-001", 5, 45.5);
    ChemicalFilter chemicalFilter = new ChemicalFilter("CHEM-001", 3, 85.0);
    
    carbonFilter.DisplayInfo();
    Console.WriteLine();
    chemicalFilter.DisplayInfo();
    
    carbonFilter.ProcessWater(1000);
    chemicalFilter.ProcessWater(800);
    
    carbonFilter.DisplayInfo();
    Console.WriteLine();
    chemicalFilter.DisplayInfo();
    
    Console.WriteLine("\n\n[DEMO 2: DivideByZeroException Handling]");
    Console.WriteLine("----------------------------------------");
    
    WaterFilter newFilter = new CarbonFilter("CARB-002", 0, 20.0);
    Console.WriteLine($"Efficiency calculation: {newFilter.CalculateEfficiency()}%");
    
    Console.WriteLine("\n\n[DEMO 3: Validation Exception Handling]");
    Console.WriteLine("----------------------------------------");
    
    try
    {
        Console.WriteLine("Attempting negative usage count...");
        WaterFilter invalidFilter = new CarbonFilter("CARB-003", -5, 30.0);
    }
    catch (ArgumentException ex)
    {
        Console.WriteLine($"✓ Caught: {ex.Message}");
    }
    
    Console.WriteLine("\n\n[DEMO 4: Critical Level Exception]");
    Console.WriteLine("----------------------------------------");
    
    try
    {
        Console.WriteLine("Attempting chemical level 150%...");
        ChemicalFilter dangerousFilter = new ChemicalFilter("CHEM-002", 1, 150.0);
    }
    catch (OverflowException ex)
    {
        Console.WriteLine($"⚠ CRITICAL: {ex.Message}");
    }
    
    Console.WriteLine("\n\n[DEMO 5: Polymorphism in Action]");
    Console.WriteLine("----------------------------------------");
    
    WaterFilter[] filters = {
        new CarbonFilter("CARB-005", 8, 25.5),
        new ChemicalFilter("CHEM-003", 6, 45.0)
    };
    
    foreach (WaterFilter filter in filters)
    {
        Console.WriteLine($"\nProcessing with {filter.FilterID} ({filter.GetType().Name}):");
        filter.ProcessWater(600);
        Console.WriteLine($"Efficiency: {filter.CalculateEfficiency()}%");
    }
}
catch (ArgumentException ex)
{
    Console.WriteLine($"\n[VALIDATION ERROR] {ex.Message}");
}
catch (DivideByZeroException ex)
{
    Console.WriteLine($"\n[DIVIDE BY ZERO ERROR] {ex.Message}");
}
catch (Exception ex)
{
    Console.WriteLine($"\n[UNEXPECTED ERROR] {ex.Message}");
}
finally
{
    Console.WriteLine("\n========================================");
    Console.WriteLine("╔════════════════════════════════╗");
    Console.WriteLine("║      SYSTEM SHUTDOWN          ║");
    Console.WriteLine("║      Session Ended            ║");
    Console.WriteLine("║      Thank you, Manalili!     ║");
    Console.WriteLine("╚════════════════════════════════╝");
    Console.WriteLine("========================================");
}

Console.WriteLine("\nPress any key to exit...");
Console.ReadKey();

public abstract class WaterFilter
{
    private string _filterID;
    private int _usageCount;

    public string FilterID
    {
        get { return _filterID; }
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Filter ID cannot be empty or null.");
            _filterID = value;
        }
    }

    public int UsageCount
    {
        get { return _usageCount; }
        set
        {
            if (value < 0)
                throw new ArgumentException("Usage count cannot be negative.");
            _usageCount = value;
        }
    }

        public WaterFilter(string filterID, int usageCount)
    {
        FilterID = filterID;
        UsageCount = usageCount;
    }

    public abstract void ProcessWater(double amount);

    public virtual double CalculateEfficiency()
    {
        try
        {
            if (UsageCount == 0)
                return 0;
            return Math.Round(100.0 / UsageCount, 2);
        }
        catch (DivideByZeroException)
        {
            Console.WriteLine($"[WARNING] Cannot calculate efficiency for {FilterID}: No water processed yet.");
            return 0;
        }
    }

    public virtual void DisplayInfo()
    {
        Console.WriteLine($"Filter ID: {FilterID}");
        Console.WriteLine($"Usage Count: {UsageCount} cycles");
    }
}

public class CarbonFilter : WaterFilter
{
    private double _debrisLevel;

    public double DebrisLevel
    {
        get { return _debrisLevel; }
        set
        {
            if (value < 0)
                _debrisLevel = 0;
            else if (value > 100)
            {
                Console.WriteLine("[ALERT] Debris level exceeds maximum! Maintenance required!");
                _debrisLevel = 100;
            }
            else
                _debrisLevel = value;
        }
    }

    public CarbonFilter(string filterID, int usageCount, double debrisLevel) 
        : base(filterID, usageCount)
    {
        DebrisLevel = debrisLevel;
    }

    public override void ProcessWater(double amount)
    {
        Console.WriteLine("\n[CARBON FILTER PROCESSING]");
        
        double debrisRemoved = amount * 0.05;
        
        if (debrisRemoved > DebrisLevel)
            debrisRemoved = DebrisLevel;
            
        DebrisLevel -= debrisRemoved;
        UsageCount++;
        
        Console.WriteLine($"  ✓ Processed {amount}L of water");
        Console.WriteLine($"  ✓ Debris removed: {debrisRemoved:F2} units");
        Console.WriteLine($"  ✓ Remaining debris level: {DebrisLevel:F2}%");
    }

    public override double CalculateEfficiency()
    {
        double baseEfficiency = base.CalculateEfficiency();
        double efficiency = baseEfficiency * (1 - (DebrisLevel / 200));
        return Math.Round(efficiency, 2);
    }

    public override void DisplayInfo()
    {
        base.DisplayInfo();
        Console.WriteLine($"Filter Type: CARBON");
        Console.WriteLine($"Current Debris Level: {DebrisLevel:F2}%");
        Console.WriteLine($"Efficiency Rating: {CalculateEfficiency():F2}%");
    }
}

public class ChemicalFilter : WaterFilter
{
    private double _chemicalLevel;

    public double ChemicalLevel
    {
        get { return _chemicalLevel; }
        set
        {
            if (value < 0)
                throw new ArgumentException("Chemical level cannot be negative.");
            if (value > 100)
            {
                throw new OverflowException("CRITICAL: Chemical level exceeds safety threshold! Emergency shutdown initiated!");
            }
            _chemicalLevel = value;
        }
    }

    public ChemicalFilter(string filterID, int usageCount, double chemicalLevel) 
        : base(filterID, usageCount)
    {
        ChemicalLevel = chemicalLevel;
    }

    public override void ProcessWater(double amount)
    {
        Console.WriteLine("\n[CHEMICAL FILTER PROCESSING]");
        
        double chemicalsUsed = amount * 0.02;
        
        if (chemicalsUsed > ChemicalLevel)
            chemicalsUsed = ChemicalLevel;
            
        ChemicalLevel -= chemicalsUsed;
        UsageCount++;
        
        if (ChemicalLevel < 10)
        {
            Console.WriteLine("  [WARNING] Chemical level running low! Refill soon!");
        }
        
        Console.WriteLine($"  ✓ Processed {amount}L of water");
        Console.WriteLine($"  ✓ Chemicals consumed: {chemicalsUsed:F2} units");
        Console.WriteLine($"  ✓ Remaining chemical level: {ChemicalLevel:F2}%");
    }

    public override double CalculateEfficiency()
    {
        double baseEfficiency = base.CalculateEfficiency();
        
        double efficiencyModifier = 1.0;
        
        if (ChemicalLevel < 30)
            efficiencyModifier = 0.7;
        else if (ChemicalLevel > 70)
            efficiencyModifier = 0.8;
        else
            efficiencyModifier = 1.2;
            
        return Math.Round(baseEfficiency * efficiencyModifier, 2);
    }

    public override void DisplayInfo()
    {
        base.DisplayInfo();
        Console.WriteLine($"Filter Type: CHEMICAL");
        Console.WriteLine($"Current Chemical Level: {ChemicalLevel:F2}%");
        Console.WriteLine($"Efficiency Rating: {CalculateEfficiency():F2}%");
    }
=======
Console.WriteLine("================================================");
Console.WriteLine("    MANALILI - BIO-SAFE WATER TREATMENT");
Console.WriteLine("    Scenario D: The Clean-Water Plant");
Console.WriteLine("================================================\n");

try
{
    // DEMO 1: Normal Operation
    Console.WriteLine("[DEMO 1: Normal Filter Operation]");
    Console.WriteLine("----------------------------------------");
    
    CarbonFilter carbonFilter = new CarbonFilter("CARB-001", 5, 45.5);
    ChemicalFilter chemicalFilter = new ChemicalFilter("CHEM-001", 3, 85.0);
    
    carbonFilter.DisplayInfo();
    Console.WriteLine();
    chemicalFilter.DisplayInfo();
    
    carbonFilter.ProcessWater(1000);
    chemicalFilter.ProcessWater(800);
    
    carbonFilter.DisplayInfo();
    Console.WriteLine();
    chemicalFilter.DisplayInfo();
    
    // DEMO 2: DivideByZeroException
    Console.WriteLine("\n\n[DEMO 2: DivideByZeroException Handling]");
    Console.WriteLine("----------------------------------------");
    
    WaterFilter newFilter = new CarbonFilter("CARB-002", 0, 20.0);
    Console.WriteLine($"Efficiency calculation: {newFilter.CalculateEfficiency()}%");
    
    // DEMO 3: Validation Exception
    Console.WriteLine("\n\n[DEMO 3: Validation Exception Handling]");
    Console.WriteLine("----------------------------------------");
    
    try
    {
        Console.WriteLine("Attempting negative usage count...");
        WaterFilter invalidFilter = new CarbonFilter("CARB-003", -5, 30.0);
    }
    catch (ArgumentException ex)
    {
        Console.WriteLine($"✓ Caught: {ex.Message}");
    }
    
    // DEMO 4: Chemical Overflow
    Console.WriteLine("\n\n[DEMO 4: Critical Level Exception]");
    Console.WriteLine("----------------------------------------");
    
    try
    {
        Console.WriteLine("Attempting chemical level 150%...");
        ChemicalFilter dangerousFilter = new ChemicalFilter("CHEM-002", 1, 150.0);
    }
    catch (OverflowException ex)
    {
        Console.WriteLine($"⚠ CRITICAL: {ex.Message}");
    }
    
    // DEMO 5: Polymorphism
    Console.WriteLine("\n\n[DEMO 5: Polymorphism in Action]");
    Console.WriteLine("----------------------------------------");
    
    WaterFilter[] filters = {
        new CarbonFilter("CARB-005", 8, 25.5),
        new ChemicalFilter("CHEM-003", 6, 45.0)
    };
    
    foreach (WaterFilter filter in filters)
    {
        Console.WriteLine($"\nProcessing with {filter.FilterID} ({filter.GetType().Name}):");
        filter.ProcessWater(600);
        Console.WriteLine($"Efficiency: {filter.CalculateEfficiency()}%");
    }
}
catch (ArgumentException ex)
{
    Console.WriteLine($"\n[VALIDATION ERROR] {ex.Message}");
}
catch (DivideByZeroException ex)
{
    Console.WriteLine($"\n[DIVIDE BY ZERO ERROR] {ex.Message}");
}
catch (Exception ex)
{
    Console.WriteLine($"\n[UNEXPECTED ERROR] {ex.Message}");
}
finally
{
    Console.WriteLine("\n========================================");
    Console.WriteLine("╔════════════════════════════════╗");
    Console.WriteLine("║      SYSTEM SHUTDOWN          ║");
    Console.WriteLine("║      Session Ended            ║");
    Console.WriteLine("║      Thank you, Manalili!     ║");
    Console.WriteLine("╚════════════════════════════════╝");
    Console.WriteLine("========================================");
}

Console.WriteLine("\nPress any key to exit...");
Console.ReadKey();

public abstract class WaterFilter
{
    private string _filterID;
    private int _usageCount;

    public string FilterID
    {
        get { return _filterID; }
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Filter ID cannot be empty or null.");
            _filterID = value;
        }
    }

    public int UsageCount
    {
        get { return _usageCount; }
        set
        {
            if (value < 0)
                throw new ArgumentException("Usage count cannot be negative.");
            _usageCount = value;
        }
    }

    public WaterFilter(string filterID, int usageCount)
    {
        FilterID = filterID;
        UsageCount = usageCount;
    }

    public abstract void ProcessWater(double amount);

    public virtual double CalculateEfficiency()
    {
        try
        {
            if (UsageCount == 0)
                return 0;
            return Math.Round(100.0 / UsageCount, 2);
        }
        catch (DivideByZeroException)
        {
            Console.WriteLine($"[WARNING] Cannot calculate efficiency for {FilterID}: No water processed yet.");
            return 0;
        }
    }

    public virtual void DisplayInfo()
    {
        Console.WriteLine($"Filter ID: {FilterID}");
        Console.WriteLine($"Usage Count: {UsageCount} cycles");
    }
}

public class CarbonFilter : WaterFilter
{
    private double _debrisLevel;

    public double DebrisLevel
    {
        get { return _debrisLevel; }
        set
        {
            if (value < 0)
                _debrisLevel = 0;
            else if (value > 100)
            {
                Console.WriteLine("[ALERT] Debris level exceeds maximum! Maintenance required!");
                _debrisLevel = 100;
            }
            else
                _debrisLevel = value;
        }
    }

    public CarbonFilter(string filterID, int usageCount, double debrisLevel) 
        : base(filterID, usageCount)
    {
        DebrisLevel = debrisLevel;
    }

    public override void ProcessWater(double amount)
    {
        Console.WriteLine("\n[CARBON FILTER PROCESSING]");
        
        double debrisRemoved = amount * 0.05;
        
        if (debrisRemoved > DebrisLevel)
            debrisRemoved = DebrisLevel;
            
        DebrisLevel -= debrisRemoved;
        UsageCount++;
        
        Console.WriteLine($"  ✓ Processed {amount}L of water");
        Console.WriteLine($"  ✓ Debris removed: {debrisRemoved:F2} units");
        Console.WriteLine($"  ✓ Remaining debris level: {DebrisLevel:F2}%");
    }

    public override double CalculateEfficiency()
    {
        double baseEfficiency = base.CalculateEfficiency();
        double efficiency = baseEfficiency * (1 - (DebrisLevel / 200));
        return Math.Round(efficiency, 2);
    }

    public override void DisplayInfo()
    {
        base.DisplayInfo();
        Console.WriteLine($"Filter Type: CARBON");
        Console.WriteLine($"Current Debris Level: {DebrisLevel:F2}%");
        Console.WriteLine($"Efficiency Rating: {CalculateEfficiency():F2}%");
    }
}

public class ChemicalFilter : WaterFilter
{
    private double _chemicalLevel;

    public double ChemicalLevel
    {
        get { return _chemicalLevel; }
        set
        {
            if (value < 0)
                throw new ArgumentException("Chemical level cannot be negative.");
            if (value > 100)
            {
                throw new OverflowException("CRITICAL: Chemical level exceeds safety threshold! Emergency shutdown initiated!");
            }
            _chemicalLevel = value;
        }
    }

    public ChemicalFilter(string filterID, int usageCount, double chemicalLevel) 
        : base(filterID, usageCount)
    {
        ChemicalLevel = chemicalLevel;
    }

    public override void ProcessWater(double amount)
    {
        Console.WriteLine("\n[CHEMICAL FILTER PROCESSING]");
        
        double chemicalsUsed = amount * 0.02;
        
        if (chemicalsUsed > ChemicalLevel)
            chemicalsUsed = ChemicalLevel;
            
        ChemicalLevel -= chemicalsUsed;
        UsageCount++;
        
        if (ChemicalLevel < 10)
        {
            Console.WriteLine("  [WARNING] Chemical level running low! Refill soon!");
        }
        
        Console.WriteLine($"  ✓ Processed {amount}L of water");
        Console.WriteLine($"  ✓ Chemicals consumed: {chemicalsUsed:F2} units");
        Console.WriteLine($"  ✓ Remaining chemical level: {ChemicalLevel:F2}%");
    }

    public override double CalculateEfficiency()
    {
        double baseEfficiency = base.CalculateEfficiency();
        
        double efficiencyModifier = 1.0;
        
        if (ChemicalLevel < 30)
            efficiencyModifier = 0.7;
        else if (ChemicalLevel > 70)
            efficiencyModifier = 0.8;
        else
            efficiencyModifier = 1.2;
            
        return Math.Round(baseEfficiency * efficiencyModifier, 2);
    }

    public override void DisplayInfo()
    {
        base.DisplayInfo();
        Console.WriteLine($"Filter Type: CHEMICAL");
        Console.WriteLine($"Current Chemical Level: {ChemicalLevel:F2}%");
        Console.WriteLine($"Efficiency Rating: {CalculateEfficiency():F2}%");
    }
>>>>>>> 548c19fce9edbd8c1a15681f931b3cdc4ada949b
}