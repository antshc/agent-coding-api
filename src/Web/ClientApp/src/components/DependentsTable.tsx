import type { GetDependentDto } from "@/types/api";
import { fullName, formatDate, calculateAge, getRelationshipLabel } from "@/lib/helpers";

interface Props {
  dependents: GetDependentDto[];
}

export function DependentsTable({ dependents }: Props) {
  if (dependents.length === 0) {
    return (
      <div className="rounded-md border border-border bg-card p-4 text-sm text-muted-foreground">
        No dependents on file.
      </div>
    );
  }

  return (
    <div className="rounded-md border border-border bg-card overflow-hidden">
      <table className="w-full text-sm">
        <thead>
          <tr className="border-b border-border bg-secondary/50">
            <th className="text-left px-4 py-2 font-medium text-muted-foreground">Name</th>
            <th className="text-left px-4 py-2 font-medium text-muted-foreground">Relationship</th>
            <th className="text-left px-4 py-2 font-medium text-muted-foreground">Date of Birth</th>
            <th className="text-right px-4 py-2 font-medium text-muted-foreground">Age</th>
            <th className="text-center px-4 py-2 font-medium text-muted-foreground">Over 50</th>
          </tr>
        </thead>
        <tbody>
          {dependents.map((d) => {
            const age = calculateAge(d.dateOfBirth);
            return (
              <tr key={d.id} className="border-b border-border last:border-0">
                <td className="px-4 py-2.5 text-card-foreground">{fullName(d.firstName, d.lastName)}</td>
                <td className="px-4 py-2.5 text-card-foreground">{getRelationshipLabel(d.relationship)}</td>
                <td className="px-4 py-2.5 text-card-foreground">{formatDate(d.dateOfBirth)}</td>
                <td className="px-4 py-2.5 text-right tabular-nums text-card-foreground">{age}</td>
                <td className="px-4 py-2.5 text-center">
                  {age > 50 ? (
                    <span className="text-[10px] font-medium px-1.5 py-0.5 rounded bg-accent text-accent-foreground">
                      Over 50
                    </span>
                  ) : (
                    <span className="text-muted-foreground">—</span>
                  )}
                </td>
              </tr>
            );
          })}
        </tbody>
      </table>
    </div>
  );
}
