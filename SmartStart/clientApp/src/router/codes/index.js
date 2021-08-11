import { All } from "@/router";
import store from "@/store";
export default [
    {
        path: "/codes",
        name: "codes",
        components: {
            default: () => import("@/views/codes/activiation-codes"),
            "bread-actions": () =>
                import(
                    "@/views/codes/activiation-codes/components/create-code.vue"
                )
        },
        props: {
            default: true
        },
        meta: () => ({
            pageTitle: "رموز التفعيل",
            roles: [All],
            breadcrumb: [
                {
                    text: "رموز التفعيل",
                    active: true
                }
            ]
        })
    },
    {
        path: "/packages",
        name: "packages",
        components: {
            default: () => import("@/views/codes/activiation-codes"),
            "bread-actions": () =>
                import(
                    "@/views/codes/activiation-codes/components/set-package.vue"
                )
        },
        props: {
            default: true
        },
        meta: () => ({
            pageTitle: "رموز التفعيل",
            roles: [All],
            breadcrumb: [
                {
                    text: "الحزم المتوفرة",
                    active: true
                }
            ]
        })
    },
    {
        path: "/packages/set/:id",
        name: "package set",
        components: {
            default: () =>
                import("@/views/codes/activiation-codes/pages/set-packages.vue")
        },
        props: {
            default: true
        },
        meta: () => ({
            pageTitle: "رموز التفعيل",
            roles: [All],
            breadcrumb: [
                {
                    text: "الحزم المتوفرة",
                    active: false,
                    to: "/packages"
                },
                {
                    text: "تفاصيل",
                    active: true
                }
            ]
        })
    },
    {
        path: "/invoice",
        name: "invoice",
        components: {
            default: () => import("@/views/codes/invoice"),
            "bread-actions": () =>
                import("@/views/codes/invoice/components/invoice-search.vue")
        },
        meta: () => ({
            pageTitle: "كشف الحساب",
            roles: [All],
            breadcrumb: [
                {
                    text: "كشف الحساب",
                    active: true
                }
            ]
        })
    },
    {
        path: "/invoice/:id",
        name: "invoice details",
        components: {
            default: () => import("@/views/codes/invoice/details.vue"),
            "bread-actions": () =>
                import("@/views/codes/invoice/components/create-invoice.vue")
        },
        props: {
            default: true,
            "bread-actions": true
        },
        meta: () => ({
            pageTitle: "كشف الحساب ",
            roles: [All],
            breadcrumb: [
                {
                    text: "كشف الحساب ",
                    active: false,
                    to: "/invoice"
                },
                {
                    text: "فواتير " + store.state.invoices.posName,
                    active: true
                }
            ]
        })
    },
    {
        path: "/invoice/:id/set/:invoiceId",
        name: "invoice question details",
        components: {
            default: () =>
                import("@/views/codes/invoice/pages/invoice-details.vue")
        },
        props: {
            default: true
        },
        meta: () => ({
            pageTitle: "كشف الحساب",
            roles: [All],
            breadcrumb: [
                {
                    text: "كشف الحساب",
                    active: false,
                    to: "/invoice"
                },
                {
                    text: "فواتير " + store.state.invoices.invoiceDto.posName,
                    active: false,
                    to: "/invoice/" + store.state.invoices.invoiceDto.posId
                },
                {
                    text:
                        store.state.invoices.invoiceDto.invoiceId ==
                        "00000000-0000-0000-0000-000000000000"
                            ? "إضافة فاتورة"
                            : "تفاصيل الفاتورة",
                    active: true
                }
            ]
        })
    },
    {
        path: "/invoice/:id/print/:invoiceId",
        name: "invoice print",
        components: {
            default: () =>
                import("@/views/codes/invoice/pages/invoice-print.vue")
        },
        props: {
            default: true
        },
        meta: () => ({
            pageTitle: "طباعة الفاتورة",
            roles: [All],
            breadcrumb: []
        })
    }
];
