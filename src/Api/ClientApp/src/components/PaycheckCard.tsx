import { useQuery } from "@tanstack/react-query";
import { fetchPaycheck } from "@/lib/api";
import { formatCurrency } from "@/lib/helpers";

interface Props {
  employeeId: number;
}

export function PaycheckCard({ employeeId }: Props) {
  const { data: paycheck, isLoading, isError, error } = useQuery({
    queryKey: ["paycheck", employeeId],
    queryFn: () => fetchPaycheck(employeeId),
  });

  if (isLoading) {
    return (
      <div className="rounded-md border border-border bg-card p-4 space-y-3">
        <div className="h-5 w-24 bg-secondary animate-pulse rounded" />
        <div className="h-16 bg-secondary animate-pulse rounded" />
      </div>
    );
  }

  if (isError) {
    return (
      <div className="rounded-md border border-destructive/30 bg-card p-4 text-sm text-destructive">
        Failed to load paycheck: {(error as Error).message}
      </div>
    );
  }

  if (!paycheck) return null;

  return (
    <div className="rounded-md border border-border bg-card p-4 space-y-4">
      <div className="flex items-center justify-between">
        <h4 className="text-sm font-semibold text-card-foreground">Paycheck Summary</h4>
        {paycheck.payPeriodTypeFriendlyName && (
          <span className="text-xs text-muted-foreground">{paycheck.payPeriodTypeFriendlyName}</span>
        )}
      </div>

      <div className="grid grid-cols-3 gap-4">
        <div>
          <p className="text-xs text-muted-foreground">Gross Pay</p>
          <p className="text-base font-semibold tabular-nums text-card-foreground">{formatCurrency(paycheck.grossAmount)}</p>
        </div>
        <div>
          <p className="text-xs text-muted-foreground">Deductions</p>
          <p className="text-base font-semibold tabular-nums text-destructive">{formatCurrency(paycheck.totalDeductions)}</p>
        </div>
        <div>
          <p className="text-xs text-muted-foreground">Net Pay</p>
          <p className="text-base font-semibold tabular-nums text-card-foreground">{formatCurrency(paycheck.netAmount)}</p>
        </div>
      </div>

      {paycheck.deductionBreakdown && paycheck.deductionBreakdown.length > 0 && (
        <div>
          <h5 className="text-xs font-medium text-muted-foreground mb-2">Deduction Breakdown</h5>
          <div className="rounded border border-border overflow-hidden">
            <table className="w-full text-sm">
              <thead>
                <tr className="bg-secondary/50 border-b border-border">
                  <th className="text-left px-3 py-1.5 font-medium text-muted-foreground text-xs">Deduction</th>
                  <th className="text-right px-3 py-1.5 font-medium text-muted-foreground text-xs">Amount</th>
                </tr>
              </thead>
              <tbody>
                {paycheck.deductionBreakdown.map((line, i) => (
                  <tr key={i} className="border-b border-border last:border-0">
                    <td className="px-3 py-1.5 text-card-foreground">{line.name ?? "—"}</td>
                    <td className="px-3 py-1.5 text-right tabular-nums text-card-foreground">{formatCurrency(line.amount)}</td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        </div>
      )}
    </div>
  );
}
