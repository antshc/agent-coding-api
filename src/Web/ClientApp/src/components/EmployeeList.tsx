import { useQuery } from "@tanstack/react-query";
import { fetchEmployees } from "@/lib/api";
import { fullName, formatCurrency } from "@/lib/helpers";
import type { GetEmployeeDto } from "@/types/api";
import { Search, RefreshCw } from "lucide-react";
import { useState } from "react";
import { AddEmployeeDialog } from "./AddEmployeeDialog";

interface Props {
  selectedId: number | null;
  onSelect: (emp: GetEmployeeDto) => void;
}

export function EmployeeList({ selectedId, onSelect }: Props) {
  const [search, setSearch] = useState("");
  const { data: employees, isLoading, isError, error, refetch } = useQuery({
    queryKey: ["employees"],
    queryFn: fetchEmployees,
  });

  const filtered = employees?.filter((e) => {
    const name = fullName(e.firstName, e.lastName).toLowerCase();
    return name.includes(search.toLowerCase());
  });

  return (
    <div className="flex flex-col h-full">
      <div className="flex items-center justify-between gap-2 px-4 py-3 border-b border-border">
        <h2 className="text-sm font-semibold text-foreground tracking-wide uppercase">Employees</h2>
        <div className="flex items-center gap-1.5">
          <AddEmployeeDialog />
          <button
            onClick={() => refetch()}
            className="p-1.5 rounded-md text-muted-foreground hover:text-foreground hover:bg-secondary transition-colors active:scale-95"
            title="Refresh"
          >
            <RefreshCw size={14} />
          </button>
        </div>
      </div>

      <div className="px-3 py-2 border-b border-border">
        <div className="relative">
          <Search size={14} className="absolute left-2.5 top-1/2 -translate-y-1/2 text-muted-foreground" />
          <input
            type="text"
            placeholder="Search employees…"
            value={search}
            onChange={(e) => setSearch(e.target.value)}
            className="w-full pl-8 pr-3 py-1.5 text-sm bg-secondary rounded-md border-none outline-none placeholder:text-muted-foreground focus:ring-1 focus:ring-ring"
          />
        </div>
      </div>

      <div className="flex-1 overflow-y-auto">
        {isLoading && (
          <div className="p-4 space-y-3">
            {[1, 2, 3, 4].map((i) => (
              <div key={i} className="h-14 rounded-md bg-secondary animate-pulse" />
            ))}
          </div>
        )}

        {isError && (
          <div className="p-4 text-sm text-destructive">
            Failed to load employees: {(error as Error).message}
          </div>
        )}

        {!isLoading && !isError && filtered?.length === 0 && (
          <div className="p-4 text-sm text-muted-foreground">No employees found.</div>
        )}

        {filtered?.map((emp) => (
          <button
            key={emp.id}
            onClick={() => onSelect(emp)}
            className={`w-full text-left px-4 py-3 border-b border-border transition-colors hover:bg-secondary/60 active:scale-[0.99] ${
              selectedId === emp.id ? "bg-secondary" : ""
            }`}
          >
            <div className="flex items-center justify-between">
              <span className="text-sm font-medium text-foreground">
                {fullName(emp.firstName, emp.lastName)}
              </span>
              {emp.salary > 80000 && (
                <span className="text-[10px] font-medium px-1.5 py-0.5 rounded bg-accent text-accent-foreground">
                  High salary
                </span>
              )}
            </div>
            <div className="flex items-center gap-3 mt-0.5 text-xs text-muted-foreground">
              <span>{formatCurrency(emp.salary)}</span>
              <span>{emp.dependents?.length ?? 0} dependents</span>
            </div>
          </button>
        ))}
      </div>
    </div>
  );
}
