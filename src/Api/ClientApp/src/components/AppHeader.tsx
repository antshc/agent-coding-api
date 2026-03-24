import { Shield } from "lucide-react";

export function AppHeader() {
  return (
    <header className="h-12 shrink-0 flex items-center gap-2.5 px-8 bg-[hsl(var(--brand))] text-[hsl(var(--brand-foreground))]">
      <Shield size={20} strokeWidth={2.2} />
      <span className="text-sm font-semibold tracking-wide">Employee Benefits</span>
    </header>
  );
}
