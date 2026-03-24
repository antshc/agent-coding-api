import { Info } from "lucide-react";

export function BenefitsRulesPanel() {
  return (
    <div className="rounded-md border border-border bg-card p-4 text-sm">
      <div className="flex items-center gap-2 mb-2">
        <Info size={14} className="text-muted-foreground" />
        <h4 className="font-semibold text-card-foreground">Benefits Rules</h4>
      </div>
      <ul className="space-y-1 text-muted-foreground list-disc list-inside">
        <li>26 paychecks per year</li>
        <li>Employee benefits base cost: $1,000/month</li>
        <li>Each dependent adds $600/month</li>
        <li>Salary above $80,000 incurs an additional 2% of yearly salary</li>
        <li>Dependents over 50 incur an additional $200/month</li>
      </ul>
      <p className="mt-3 text-xs text-muted-foreground italic">
        Paycheck amounts are calculated by the backend API. This section is informational only.
      </p>
    </div>
  );
}
