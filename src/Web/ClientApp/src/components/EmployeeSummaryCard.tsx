import { useState } from "react";
import { useQueryClient } from "@tanstack/react-query";
import type { GetEmployeeDto } from "@/types/api";
import { fullName, formatCurrency, formatDate, calculateAge } from "@/lib/helpers";
import { deleteEmployee } from "@/lib/api";
import { Button } from "@/components/ui/button";
import {
  AlertDialog,
  AlertDialogAction,
  AlertDialogCancel,
  AlertDialogContent,
  AlertDialogDescription,
  AlertDialogFooter,
  AlertDialogHeader,
  AlertDialogTitle,
} from "@/components/ui/alert-dialog";
import { Pencil, Trash2 } from "lucide-react";
import { toast } from "sonner";

interface Props {
  employee: GetEmployeeDto;
  onEdit?: () => void;
  onDeleted?: () => void;
}

export function EmployeeSummaryCard({ employee, onEdit, onDeleted }: Props) {
  const queryClient = useQueryClient();
  const [confirmOpen, setConfirmOpen] = useState(false);
  const [deleting, setDeleting] = useState(false);
  const deps = employee.dependents ?? [];
  const over50 = deps.filter((d) => calculateAge(d.dateOfBirth) > 50).length;
  const name = fullName(employee.firstName, employee.lastName);

  async function handleDelete() {
    setDeleting(true);
    try {
      await deleteEmployee(employee.id);
      await queryClient.invalidateQueries({ queryKey: ["employees"] });
      toast.success(`${name} has been removed`);
      onDeleted?.();
    } catch (err) {
      toast.error((err as Error).message);
    } finally {
      setDeleting(false);
      setConfirmOpen(false);
    }
  }

  return (
    <>
      <div className="rounded-md border border-border bg-card p-4">
        <div className="flex items-center justify-between">
          <h3 className="text-lg font-semibold text-card-foreground">{name}</h3>
          <div className="flex items-center gap-1.5">
            {onEdit && (
              <Button variant="outline" size="sm" className="gap-1.5 text-xs" onClick={onEdit}>
                <Pencil size={13} /> Edit
              </Button>
            )}
            <Button
              variant="outline"
              size="sm"
              className="gap-1.5 text-xs text-destructive hover:bg-destructive/10 hover:text-destructive border-destructive/30"
              onClick={() => setConfirmOpen(true)}
            >
              <Trash2 size={13} /> Delete
            </Button>
          </div>
        </div>
        <dl className="mt-3 grid grid-cols-2 gap-x-6 gap-y-2 text-sm">
          <div>
            <dt className="text-muted-foreground">Employee ID</dt>
            <dd className="font-medium text-card-foreground">{employee.id}</dd>
          </div>
          <div>
            <dt className="text-muted-foreground">Annual Salary</dt>
            <dd className="font-medium text-card-foreground">{formatCurrency(employee.salary)}</dd>
          </div>
          <div>
            <dt className="text-muted-foreground">Date of Birth</dt>
            <dd className="font-medium text-card-foreground">{formatDate(employee.dateOfBirth)}</dd>
          </div>
          <div>
            <dt className="text-muted-foreground">Dependents</dt>
            <dd className="font-medium text-card-foreground">{deps.length}</dd>
          </div>
        </dl>

        <div className="mt-4 flex gap-3 text-xs">
          {employee.salary > 80000 && (
            <span className="px-2 py-1 rounded bg-accent text-accent-foreground font-medium">
              Salary &gt; $80k
            </span>
          )}
          {over50 > 0 && (
            <span className="px-2 py-1 rounded bg-accent text-accent-foreground font-medium">
              {over50} dependent{over50 > 1 ? "s" : ""} over 50
            </span>
          )}
        </div>
      </div>

      <AlertDialog open={confirmOpen} onOpenChange={setConfirmOpen}>
        <AlertDialogContent>
          <AlertDialogHeader>
            <AlertDialogTitle>Delete {name}?</AlertDialogTitle>
            <AlertDialogDescription>
              This will permanently remove {name} and all associated data including dependents and paycheck records. This action cannot be undone.
            </AlertDialogDescription>
          </AlertDialogHeader>
          <AlertDialogFooter>
            <AlertDialogCancel disabled={deleting}>Cancel</AlertDialogCancel>
            <AlertDialogAction
              onClick={handleDelete}
              disabled={deleting}
              className="bg-destructive text-destructive-foreground hover:bg-destructive/90"
            >
              {deleting ? "Deleting…" : "Delete"}
            </AlertDialogAction>
          </AlertDialogFooter>
        </AlertDialogContent>
      </AlertDialog>
    </>
  );
}
