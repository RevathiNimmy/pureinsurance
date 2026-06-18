# Architecture Template: React — Web & Mobile

## Overview

A single React codebase that delivers expert-quality experiences across web browsers, iOS, and Android. The architecture prioritizes responsive design, accessibility (WCAG 2.2), internationalization, and a shared component library — so every platform gets the same quality without duplicating effort.

Two approaches exist for sharing code across web and mobile. Choose based on the company's needs:

### Approach 1: React (Web) + React Native (Mobile) with Shared Core

Separate UI layers for web and mobile, sharing business logic, API clients, state management, types, and validation in a monorepo. Web uses React DOM with Tailwind CSS. Mobile uses React Native with Expo.

```
monorepo/
├── packages/
│   ├── core/              ← shared business logic, API clients, types, validation
│   ├── ui/                ← shared design tokens, component interfaces
│   ├── i18n/              ← shared translation strings and config
│   └── state/             ← shared state management (Zustand, TanStack Query)
├── apps/
│   ├── web/               ← React DOM + Tailwind CSS + Vite
│   └── mobile/            ← React Native + Expo
└── tools/
    └── storybook/         ← component documentation
```

- Pros: Each platform gets its best-in-class UI toolkit. Web gets full CSS/Tailwind power. Mobile gets native components and gestures. No compromises on either platform.
- Cons: Two UI layers to maintain. Components aren't directly shared — only logic and tokens.
- Best for: Applications where web and mobile experiences differ significantly (e.g., complex dashboards on web, simplified mobile views).

### Approach 2: React Native + Expo (Universal — Web, iOS, Android)

Single UI codebase using React Native components that render to web (via React Native Web) and native mobile. Expo handles builds, OTA updates, and platform abstraction.

```
monorepo/
├── packages/
│   ├── ui/                ← shared React Native components (render on all platforms)
│   ├── i18n/              ← shared translations
│   └── state/             ← shared state management
├── apps/
│   └── universal/         ← Expo app targeting web + iOS + Android
└── tools/
    └── storybook/         ← component documentation
```

- Pros: Maximum code sharing (up to 90%+ across platforms). Single component library. Faster development. Expo handles builds and OTA updates.
- Cons: Web experience is constrained by React Native's component model — no direct CSS, limited web-specific patterns. Complex web layouts (data tables, rich dashboards) are harder. Expo adds a dependency layer.
- Best for: Applications where mobile is primary and web is secondary, or where the experience is similar across platforms (forms, lists, detail views, workflows).

### Recommendation

**Default to Approach 1** for the companies in this program — most are enterprise B2B applications with complex web dashboards that would be constrained by React Native Web. Use Approach 2 when the company's product is mobile-first or the web and mobile experiences are nearly identical.

## React 19 & React Compiler

All new projects should target React 19. Key changes that affect architecture decisions:

**React Compiler (stable as of late 2025):** Automatically optimizes re-renders at compile time. Manual `useMemo`, `useCallback`, and `React.memo` are no longer necessary in most cases — the compiler handles memoization automatically. This simplifies component code significantly. Enable via Babel/SWC plugin in your build config.

**Actions:** New pattern for form handling and mutations. `useActionState` and `useFormStatus` replace manual form state management. Works with both client and server rendering.

**`use` hook:** Read promises and context in render. Enables cleaner async data patterns, especially with Suspense boundaries.

**`useOptimistic`:** Built-in optimistic UI updates — show the expected result immediately while the mutation is in flight, roll back on failure.

**Server Components (RSC):** Components that execute on the server and send rendered output to the client — zero JavaScript shipped for server components. RSC requires a framework (Next.js, Remix). See Framework Choice below for when to use RSC vs SPA.

## Framework Choice

### Vite SPA (Recommended Default)

Single-page application built with Vite, served as static files from nginx containers in Kubernetes. This is the simplest deployment model and aligns directly with the infrastructure architecture templates (nginx → Istio → CDN).

- Pros: Simplest deployment (static files). Fastest dev server. No Node.js runtime needed in production. Works perfectly with the Kubernetes architecture. Full control over routing and data fetching.
- Cons: No server-side rendering (slower first paint, weaker SEO). No React Server Components. Client downloads full JavaScript bundle before rendering.
- Best for: Internal enterprise applications, admin dashboards, B2B tools — where SEO doesn't matter and users are authenticated. This covers most of the companies in this program.

### Next.js (When SSR/RSC is Needed)

Full-stack React framework with server-side rendering, static generation, and React Server Components. Requires a Node.js container instead of nginx in Kubernetes.

- Pros: Server-side rendering for fast first paint and SEO. React Server Components reduce client JavaScript. API routes built in. Image optimization. Incremental Static Regeneration.
- Cons: More complex deployment (Node.js container, not static files). Heavier runtime. Tighter coupling to Vercel's ecosystem. More operational complexity in Kubernetes.
- Best for: Public-facing applications where SEO matters, content-heavy sites, applications that benefit from server-side data fetching.

### Remix (Alternative to Next.js)

Web-standards-focused React framework. Uses Vite as its bundler. Emphasizes progressive enhancement and web platform APIs (Fetch, FormData, Response).

- Pros: Cleaner data loading model (loaders/actions per route). Progressive enhancement — forms work without JavaScript. Lighter than Next.js. Uses web standards.
- Cons: Smaller ecosystem than Next.js. Fewer deployment integrations. Also requires Node.js container.
- Best for: Teams that prefer web-standards-first approach. Applications with heavy form interactions.

### Decision

| Scenario | Framework |
|---|---|
| Internal B2B app, admin dashboard, authenticated users | Vite SPA |
| Public-facing product, SEO required | Next.js |
| Content-heavy site with dynamic data | Next.js |
| Form-heavy app, progressive enhancement valued | Remix |
| Mobile-first universal app | Expo (see Approach 2 above) |

## Design System & Component Library

### Foundation: Headless Components + Tailwind CSS (Web)

Use headless component libraries that provide behavior, accessibility, and keyboard interactions without styling. You control the visual layer entirely.

| Library | What It Provides |
|---|---|
| Radix UI | Headless primitives — dialogs, dropdowns, tooltips, tabs, popovers. Full ARIA compliance. Powers shadcn/ui. |
| Headless UI | Tailwind Labs' headless components — menus, listboxes, comboboxes, transitions. Designed for Tailwind. |
| Tailwind CSS | Utility-first CSS framework. Responsive breakpoints, dark mode, design tokens via config. |
| shadcn/ui | Pre-built components using Radix + Tailwind. Copy-paste into your project — you own the code, no dependency. |

For React Native (mobile), use a component library like Tamagui or NativeWind (Tailwind for React Native) to share design tokens across platforms.

### Design Tokens

Define colors, spacing, typography, border radii, and shadows as tokens in a shared package. Both web (Tailwind config) and mobile (theme provider) consume the same tokens:

```
packages/ui/tokens/
├── colors.ts          ← brand colors, semantic colors (success, error, warning)
├── spacing.ts         ← 4px grid system
├── typography.ts      ← font families, sizes, weights, line heights
└── breakpoints.ts     ← responsive breakpoints (sm, md, lg, xl)
```

### Storybook

Every component is documented in Storybook with:

- All visual states (default, hover, active, disabled, error, loading)
- Responsive previews (mobile, tablet, desktop viewports)
- Accessibility checks (Storybook a11y addon runs axe-core on every story)
- Internationalization previews (toggle between languages to verify layout)

Storybook is deployed as a static site — designers, PMs, and QA can browse components without running the app.

## Responsive Design

### Breakpoint Strategy

Mobile-first design. Base styles target mobile, then layer on complexity for larger screens:

| Breakpoint | Width | Target |
|---|---|---|
| Default | 0px+ | Mobile phones (portrait) |
| `sm` | 640px+ | Large phones (landscape), small tablets |
| `md` | 768px+ | Tablets |
| `lg` | 1024px+ | Laptops, small desktops |
| `xl` | 1280px+ | Desktops |
| `2xl` | 1536px+ | Large monitors |

### Layout Patterns

- **Stack → Grid:** Single column on mobile, multi-column grid on desktop. CSS Grid or Flexbox, not fixed widths.
- **Navigation:** Bottom tab bar on mobile, sidebar on desktop. Hamburger menu on tablet as transition.
- **Data Tables:** Card layout on mobile (one card per row), full table on desktop. Use responsive table components that transform automatically.
- **Forms:** Full-width inputs on mobile, multi-column layouts on desktop. Touch-friendly input sizes (minimum 44px tap targets per WCAG 2.2).
- **Typography:** Fluid type scaling using `clamp()` — text sizes adjust smoothly between breakpoints.

### Container Queries

For component-level responsiveness (a component adapts to its container width, not the viewport), use CSS Container Queries. This is critical for components that appear in different layout contexts (sidebar vs. main content vs. modal).

## Internationalization (i18n)

### Library: react-i18next

react-i18next is the recommended library. It supports React DOM and React Native, has the largest ecosystem, and handles the full range of i18n needs:

| Feature | How |
|---|---|
| Translation strings | JSON files per language, loaded lazily by namespace |
| Pluralization | ICU plural rules (one, few, many, other) |
| Interpolation | Variables in translation strings: `"Hello, {{name}}"` |
| Date/number formatting | Intl API (built into browsers and React Native) |
| RTL support | CSS `direction: rtl` + logical properties (`margin-inline-start` instead of `margin-left`) |
| Language detection | Browser language, user preference, URL path |
| Namespace splitting | Load only the translations needed for the current page/feature |

### Translation Workflow

```
Developer writes code with translation keys
    → Keys extracted to JSON (i18next-parser)
    → JSON uploaded to translation management system (Locize, Crowdin, Phrase)
    → Translators work in the TMS
    → Translated JSON pulled back into the repo (CI job)
    → App loads translations at runtime by locale
```

### RTL (Right-to-Left) Support

For Arabic, Hebrew, and other RTL languages:

- Use CSS logical properties (`margin-inline-start`, `padding-block-end`) instead of physical properties (`margin-left`, `padding-bottom`).
- Tailwind CSS supports RTL via the `rtl:` variant.
- Test with `dir="rtl"` on the root element — the entire layout should mirror.
- Icons with directional meaning (arrows, chevrons) must flip in RTL.

### Locale-Aware Formatting

Never hardcode date, number, or currency formats. Use the `Intl` API:

- `Intl.DateTimeFormat` for dates
- `Intl.NumberFormat` for numbers and currencies
- `Intl.RelativeTimeFormat` for "3 days ago" style strings

These are built into every modern browser and React Native — no library needed.

## Accessibility (WCAG 2.2 Level AA)

### Non-Negotiable Standards

The European Accessibility Act (EAA) is now in force. ADA litigation in the US continues to increase. WCAG 2.2 Level AA is the baseline for all applications.

### Implementation

| Concern | Approach |
|---|---|
| Semantic HTML | Use native elements (`button`, `nav`, `main`, `form`, `table`) — not `div` with click handlers |
| ARIA | Use headless component libraries (Radix, Headless UI) that handle ARIA correctly. Don't hand-roll ARIA. |
| Keyboard navigation | All interactive elements reachable and operable via keyboard. Visible focus indicators (WCAG 2.4.7, 2.4.11). |
| Color contrast | Minimum 4.5:1 for normal text, 3:1 for large text. Enforce via design tokens. |
| Touch targets | Minimum 24x24px (WCAG 2.5.8), recommended 44x44px for mobile. |
| Focus management | Programmatic focus on route changes (SPA), modal open/close, dynamic content. |
| Screen reader testing | Test with VoiceOver (macOS/iOS), NVDA (Windows), TalkBack (Android). |
| Automated testing | axe-core in Storybook, eslint-plugin-jsx-a11y in CI, Lighthouse CI for audits. |

### Accessibility in CI

Every PR runs:

1. `eslint-plugin-jsx-a11y` — catches common accessibility issues in JSX at lint time.
2. Storybook a11y addon — runs axe-core against every component story.
3. Lighthouse CI — audits accessibility score on key pages. Fail the build if score drops below threshold.

## State Management

### Recommended Stack

| Concern | Tool | Why |
|---|---|---|
| Server state | TanStack Query (React Query) | Caching, background refetching, optimistic updates, pagination. Eliminates most Redux use cases. |
| Client state | Zustand | Minimal, no boilerplate, works in React DOM and React Native. For UI state that doesn't come from the server. |
| Forms | React Hook Form + Zod | Performant form handling with schema-based validation. Zod schemas shared between frontend and backend. |
| URL state | URL search params (web) | Filters, pagination, sort order — keep in the URL so it's shareable and bookmarkable. |

### What NOT to Use

Avoid Redux for new projects — TanStack Query + Zustand covers the same ground with far less boilerplate. Avoid global state for server data — let TanStack Query manage it. With React 19's Compiler handling memoization automatically, the performance arguments for Redux's selector pattern are largely eliminated.

## Monorepo Tooling

| Tool | Purpose |
|---|---|
| Turborepo or Nx | Monorepo build orchestration — caching, task dependencies, parallel execution |
| pnpm | Package manager — fast, disk-efficient, strict dependency resolution |
| TypeScript | Shared types across all packages and apps — single source of truth for API contracts |
| Vite | Web app bundler — fast dev server, optimized production builds |
| Expo | Mobile app builds, OTA updates, native module management |
| Storybook | Component documentation and visual testing |
| Vitest | Unit and component testing (Vite-native, fast) |
| Playwright | End-to-end testing for web |
| Detox | End-to-end testing for mobile (React Native) |

## Deployment

### Web

React web apps build to static files, served from nginx containers inside Kubernetes (as defined in the AWS/Azure/Hybrid architecture templates). CDN in front for caching. The CI/CD pipeline (from the ci-cd docs) handles build → image push → GitOps deployment.

### Mobile

Expo EAS (Expo Application Services) handles mobile builds and distribution:

- **EAS Build** — cloud builds for iOS and Android (no local Xcode/Android Studio needed).
- **EAS Submit** — automated submission to App Store and Google Play.
- **EAS Update** — over-the-air JavaScript updates without app store review (for non-native changes).

Mobile CI/CD integrates with the same Git platform (GitHub Actions triggers EAS builds on release tags).
