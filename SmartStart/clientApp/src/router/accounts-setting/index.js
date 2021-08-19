import { All } from "@/router";
export default [
    {
        path: "/users/0",
        name: "application-users",
        components: {
            default: () => import("@/views/accounts-setting"),
            'bread-actions' : () => import("@/views/accounts-setting/application-users/components/create-application-users.vue"),
        },
        props: {
            default: true
        },
        meta: () => ({
                    pageTitle: "مستخدمو التطبيق",
                    roles: [All],
                    breadcrumb: [
                        {
                            text: "مستخدمو التطبيق",
                            active: true
                        }
                    ]
               
            })
    },
    {
        path: "/users/1",
        name: "point-sales",
        components: {
            default: () => import("@/views/accounts-setting"),
            'bread-actions' : () => import("@/views/accounts-setting/points-sales/components/create-points-sales.vue")
        },
        props: {
            default: true
        },
        meta: () => ({
                pageTitle: "نقاط البيع",
                roles: [All],
                breadcrumb: [
                    {
                        text: "نقاط البيع",
                        active: true
                    }
                ]
           
       })
    
    },
    {
        path: "/users/2",
        name: "panel-users",
        components: {
            default: () => import("@/views/accounts-setting"),
            'bread-actions' : () => import("@/views/accounts-setting/panel-users/components/create-panel-users.vue"),
        },
        props: {
            default: true
        },
        meta: () => ({
                    pageTitle: "مستخدمو اللوحة",
                    roles: [All],
                    breadcrumb: [
                        {
                            text: "مستخدمو اللوحة",
                            active: true
                        }
                    ]
                
        })
    },
    {
        path: "/users/0/details/:id",
        name: "application-users details",
        components: {
            default: () =>
                import("@/views/accounts-setting/application-users/details.vue")
        },
        props: {
            default: true
        },
        meta: () => ({
            pageTitle: "مستخدمو التطبيق",
            roles: [All],
            breadcrumb: [
                {
                    text: "مستخدمو التطبيق",
                    active: false,
                    to: "/users/0"
                },
                {
                    text: "تفاصيل",
                    active: true
                }
            ]
        })
    },
    {
        path: "/users/1/details/:id",
        name: "points-sales details",
        components: {
            default: () =>
                import("@/views/accounts-setting/points-sales/details.vue")
        },
        props: {
            default: true
        },
        meta: () => ({
            pageTitle: " نقاط البيع",
            roles: [All],
            breadcrumb: [
                {
                    text: " نقاط البيع",
                    active: false,
                    to: "/users/1"
                },
                {
                    text: "تفاصيل",
                    active: true
                }
            ]
        })
    },
    {
        path: "/users/2/details/:id",
        name: "panel-users details",
        components: {
            default: () =>
                import("@/views/accounts-setting/panel-users/details.vue")
        },
        props: {
            default: true
        },
        meta: () => ({
            pageTitle: "مستخدمو اللوحة",
            roles: [All],
            breadcrumb: [
                {
                    text: "مستخدمو اللوحة",
                    active: false,
                    to: "/users/2"
                },
                {
                    text: "تفاصيل",
                    active: true
                }
            ]
        })
    }
];
