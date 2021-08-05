import { All } from "@/router";
export default [
    {
        path: "/docs",
        name: "docs",
        component: () => import("@/EK-core/docs"),
        redirect: '/docs/forms',
        children: [
            {
                path: 'tables',
                component: () => import("@/EK-core/docs/tables"),
                redirect: '/docs/tables/basic',
                children: [
                    {
                        path: 'basic',
                        name: "basic",
                        component: () => import("@/EK-core/docs/tables/basic"),
                        meta: () => ({
                            pageTitle: "basic table",
                            roles: [All],
                            layout: "full"
                        }),
                    },
                    {
                        path: 'collapse',
                        name: "collapse",
                        component: () => import("@/EK-core/docs/tables/collapse"),
                        meta: () => ({
                            pageTitle: "collapse table",
                            roles: [All],
                            layout: "full"
                        }),
                    }
                ]
            },
            {
                path: 'dialog',
                name: "dialog",
                component: () => import("@/EK-core/docs/dialog"),
                meta: () => ({
                    pageTitle: "dialog",
                    roles: [All],
                    layout: "full"
                })
            },
            {
                path: 'forms',
                name: "forms",
                component: () => import("@/EK-core/docs/forms"),
                meta: () => ({
                    pageTitle: "forms",
                    roles: [All],
                    layout: "full"
                })
            },
        ]
    }
];
