import { useState, useEffect } from "react";
import { useQueryClient } from "@tanstack/react-query";
import { z } from "zod";
import { updateEmployee } from "@/lib/api";
import type { GetEmployeeDto, Relationship } from "@/types/api";
import {
  Dialog,
  DialogContent,
  DialogHeader,
  DialogTitle,
} from "@/components/ui/dialog";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "@/components/ui/select";
import { Plus, Trash2 } from "lucide-react";
import { toast } from "sonner";

const dependentSchema = z.object({
  firstName: z.string().trim().min(1, "First name is required").max(100),
  lastName: z.string().trim().min(1, "Last name is required").max(100),
  dateOfBirth: z.string().min(1, "Date of birth is required"),
  relationship: z.number().min(1, "Relationship is required").max(3),
});

const employeeSchema = z.object({
  firstName: z.string().trim().min(1, "First name is required").max(100),
  lastName: z.string().trim().min(1, "Last name is required").max(100),
  salary: z.number().positive("Salary must be positive").max(1_000_000_000),
  dateOfBirth: z.string().min(1, "Date of birth is required"),
  dependents: z.array(dependentSchema),
});

interface DependentForm {
  firstName: string;
  lastName: string;
  dateOfBirth: string;
  relationship: string;
}

interface Props {
  employee: GetEmployeeDto;
  open: boolean;
  onOpenChange: (open: boolean) => void;
  onUpdated: (emp: GetEmployeeDto) => void;
}

const emptyDependent = (): DependentForm => ({
  firstName: "",
  lastName: "",
  dateOfBirth: "",
  relationship: "",
});

export function EditEmployeeDialog({ employee, open, onOpenChange, onUpdated }: Props) {
  const queryClient = useQueryClient();
  const [submitting, setSubmitting] = useState(false);
  const [errors, setErrors] = useState<Record<string, string>>({});

  const [firstName, setFirstName] = useState("");
  const [lastName, setLastName] = useState("");
  const [salary, setSalary] = useState("");
  const [dateOfBirth, setDateOfBirth] = useState("");
  const [dependents, setDependents] = useState<DependentForm[]>([]);

  // Populate form when employee changes or dialog opens
  useEffect(() => {
    if (open) {
      setFirstName(employee.firstName ?? "");
      setLastName(employee.lastName ?? "");
      setSalary(String(employee.salary));
      setDateOfBirth(employee.dateOfBirth);
      setDependents(
        (employee.dependents ?? []).map((d) => ({
          firstName: d.firstName ?? "",
          lastName: d.lastName ?? "",
          dateOfBirth: d.dateOfBirth,
          relationship: String(d.relationship),
        }))
      );
      setErrors({});
    }
  }, [open, employee]);

  function updateDependent(index: number, field: keyof DependentForm, value: string) {
    setDependents((prev) =>
      prev.map((d, i) => (i === index ? { ...d, [field]: value } : d))
    );
  }

  async function handleSubmit(e: React.FormEvent) {
    e.preventDefault();
    setErrors({});

    const raw = {
      firstName,
      lastName,
      salary: parseFloat(salary),
      dateOfBirth,
      dependents: dependents.map((d) => ({
        firstName: d.firstName,
        lastName: d.lastName,
        dateOfBirth: d.dateOfBirth,
        relationship: parseInt(d.relationship) || 0,
      })),
    };

    const result = employeeSchema.safeParse(raw);
    if (!result.success) {
      const fieldErrors: Record<string, string> = {};
      result.error.errors.forEach((err) => {
        const path = err.path.join(".");
        if (!fieldErrors[path]) fieldErrors[path] = err.message;
      });
      setErrors(fieldErrors);
      return;
    }

    setSubmitting(true);
    try {
      const updated = await updateEmployee(employee.id, {
        firstName: result.data.firstName,
        lastName: result.data.lastName,
        salary: result.data.salary,
        dateOfBirth: result.data.dateOfBirth,
        dependents: result.data.dependents.map((d) => ({
          firstName: d.firstName,
          lastName: d.lastName,
          dateOfBirth: d.dateOfBirth,
          relationship: d.relationship as Relationship,
        })),
      });
      await queryClient.invalidateQueries({ queryKey: ["employees"] });
      await queryClient.invalidateQueries({ queryKey: ["paycheck", employee.id] });
      toast.success("Employee updated successfully");
      onUpdated(updated);
      onOpenChange(false);
    } catch (err) {
      toast.error((err as Error).message);
    } finally {
      setSubmitting(false);
    }
  }

  return (
    <Dialog open={open} onOpenChange={onOpenChange}>
      <DialogContent className="sm:max-w-lg max-h-[85vh] overflow-y-auto">
        <DialogHeader>
          <DialogTitle>Edit Employee</DialogTitle>
        </DialogHeader>

        <form onSubmit={handleSubmit} className="space-y-4 pt-2">
          {/* Name row */}
          <div className="grid grid-cols-2 gap-3">
            <div className="space-y-1.5">
              <Label htmlFor="edit-first">First Name</Label>
              <Input id="edit-first" value={firstName} onChange={(e) => setFirstName(e.target.value)} />
              {errors.firstName && <p className="text-xs text-destructive">{errors.firstName}</p>}
            </div>
            <div className="space-y-1.5">
              <Label htmlFor="edit-last">Last Name</Label>
              <Input id="edit-last" value={lastName} onChange={(e) => setLastName(e.target.value)} />
              {errors.lastName && <p className="text-xs text-destructive">{errors.lastName}</p>}
            </div>
          </div>

          {/* Salary & DOB */}
          <div className="grid grid-cols-2 gap-3">
            <div className="space-y-1.5">
              <Label htmlFor="edit-salary">Annual Salary ($)</Label>
              <Input id="edit-salary" type="number" step="0.01" min="0" value={salary} onChange={(e) => setSalary(e.target.value)} />
              {errors.salary && <p className="text-xs text-destructive">{errors.salary}</p>}
            </div>
            <div className="space-y-1.5">
              <Label htmlFor="edit-dob">Date of Birth</Label>
              <Input id="edit-dob" type="date" value={dateOfBirth} onChange={(e) => setDateOfBirth(e.target.value)} />
              {errors.dateOfBirth && <p className="text-xs text-destructive">{errors.dateOfBirth}</p>}
            </div>
          </div>

          {/* Dependents */}
          <div className="space-y-2">
            <div className="flex items-center justify-between">
              <Label className="text-sm font-semibold">Dependents</Label>
              <Button type="button" variant="outline" size="sm" className="gap-1 text-xs h-7" onClick={() => setDependents((p) => [...p, emptyDependent()])}>
                <Plus size={12} /> Add Dependent
              </Button>
            </div>

            {dependents.length === 0 && (
              <p className="text-xs text-muted-foreground py-2">No dependents.</p>
            )}

            {dependents.map((dep, i) => (
              <div key={i} className="rounded-md border border-border p-3 space-y-2 relative">
                <button
                  type="button"
                  onClick={() => setDependents((p) => p.filter((_, idx) => idx !== i))}
                  className="absolute top-2 right-2 p-1 rounded text-muted-foreground hover:text-destructive hover:bg-destructive/10 transition-colors active:scale-95"
                  title="Remove dependent"
                >
                  <Trash2 size={14} />
                </button>
                <div className="grid grid-cols-2 gap-2">
                  <div className="space-y-1">
                    <Label className="text-xs">First Name</Label>
                    <Input value={dep.firstName} onChange={(e) => updateDependent(i, "firstName", e.target.value)} className="h-8 text-sm" />
                    {errors[`dependents.${i}.firstName`] && <p className="text-xs text-destructive">{errors[`dependents.${i}.firstName`]}</p>}
                  </div>
                  <div className="space-y-1">
                    <Label className="text-xs">Last Name</Label>
                    <Input value={dep.lastName} onChange={(e) => updateDependent(i, "lastName", e.target.value)} className="h-8 text-sm" />
                    {errors[`dependents.${i}.lastName`] && <p className="text-xs text-destructive">{errors[`dependents.${i}.lastName`]}</p>}
                  </div>
                </div>
                <div className="grid grid-cols-2 gap-2">
                  <div className="space-y-1">
                    <Label className="text-xs">Date of Birth</Label>
                    <Input type="date" value={dep.dateOfBirth} onChange={(e) => updateDependent(i, "dateOfBirth", e.target.value)} className="h-8 text-sm" />
                    {errors[`dependents.${i}.dateOfBirth`] && <p className="text-xs text-destructive">{errors[`dependents.${i}.dateOfBirth`]}</p>}
                  </div>
                  <div className="space-y-1">
                    <Label className="text-xs">Relationship</Label>
                    <Select value={dep.relationship} onValueChange={(v) => updateDependent(i, "relationship", v)}>
                      <SelectTrigger className="h-8 text-sm">
                        <SelectValue placeholder="Select…" />
                      </SelectTrigger>
                      <SelectContent>
                        <SelectItem value="1">Spouse</SelectItem>
                        <SelectItem value="2">Domestic Partner</SelectItem>
                        <SelectItem value="3">Child</SelectItem>
                      </SelectContent>
                    </Select>
                    {errors[`dependents.${i}.relationship`] && <p className="text-xs text-destructive">{errors[`dependents.${i}.relationship`]}</p>}
                  </div>
                </div>
              </div>
            ))}
          </div>

          {/* Submit */}
          <div className="flex justify-end gap-2 pt-2">
            <Button type="button" variant="outline" onClick={() => onOpenChange(false)} disabled={submitting}>
              Cancel
            </Button>
            <Button type="submit" disabled={submitting}>
              {submitting ? "Saving…" : "Save Changes"}
            </Button>
          </div>
        </form>
      </DialogContent>
    </Dialog>
  );
}
