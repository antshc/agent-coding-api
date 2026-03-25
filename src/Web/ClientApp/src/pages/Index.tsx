import { useState, useEffect } from "react";
import { useQuery } from "@tanstack/react-query";
import { fetchEmployees } from "@/lib/api";
import type { GetEmployeeDto } from "@/types/api";
import { EmployeeList } from "@/components/EmployeeList";
import { EmployeeSummaryCard } from "@/components/EmployeeSummaryCard";
import { DependentsTable } from "@/components/DependentsTable";
import { PaycheckCard } from "@/components/PaycheckCard";
import { BenefitsRulesPanel } from "@/components/BenefitsRulesPanel";
import { EditEmployeeDialog } from "@/components/EditEmployeeDialog";
import { AppHeader } from "@/components/AppHeader";

const Index = () => {
  const [selected, setSelected] = useState<GetEmployeeDto | null>(null);
  const [editOpen, setEditOpen] = useState(false);

  const { data: employees } = useQuery({
    queryKey: ["employees"],
    queryFn: fetchEmployees,
  });

  // Auto-select first employee
  useEffect(() => {
    if (!selected && employees && employees.length > 0) {
      setSelected(employees[0]);
    }
  }, [employees, selected]);

  // Keep selected employee in sync with refreshed data
  useEffect(() => {
    if (selected && employees) {
      const fresh = employees.find((e) => e.id === selected.id);
      if (fresh && fresh !== selected) setSelected(fresh);
    }
  }, [employees, selected]);

  return (
    <div className="flex flex-col h-screen bg-background">
      <AppHeader />
      <div className="flex flex-1 min-h-0">
      {/* Sidebar */}
      <aside className="w-80 shrink-0 border-r border-border bg-card flex flex-col overflow-hidden">
        <EmployeeList selectedId={selected?.id ?? null} onSelect={setSelected} />
      </aside>

      {/* Main content */}
      <main className="flex-1 overflow-y-auto">
        {!selected ? (
          <div className="flex items-center justify-center h-full text-muted-foreground text-sm">
            Select an employee to view details.
          </div>
        ) : (
          <div className="max-w-3xl mx-auto p-6 space-y-5">
            <EmployeeSummaryCard employee={selected} onEdit={() => setEditOpen(true)} onDeleted={() => setSelected(null)} />

            <section>
              <h3 className="text-sm font-semibold text-foreground mb-2">Dependents</h3>
              <DependentsTable dependents={selected.dependents ?? []} />
            </section>

            <section>
              <h3 className="text-sm font-semibold text-foreground mb-2">Paycheck</h3>
              <PaycheckCard employeeId={selected.id} />
            </section>

            <BenefitsRulesPanel />

            <EditEmployeeDialog
              employee={selected}
              open={editOpen}
              onOpenChange={setEditOpen}
              onUpdated={setSelected}
            />
          </div>
        )}
      </main>
      </div>
    </div>
  );
};

export default Index;
