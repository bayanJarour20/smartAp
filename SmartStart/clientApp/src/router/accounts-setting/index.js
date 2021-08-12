import { All } from "@/router";
export default [
    {
        path: "/users/0",
        name: "application-users",
        components: {
            default: () => import("@/views/accounts-setting/index.vue"),
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
];
