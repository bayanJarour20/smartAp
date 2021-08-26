import { All } from "@/router";
// import store from "@/store";
export default [
    // --- faculties
    {
        path: "/faculties",
        name: "faculties",
        components: {
            default: () => import("@/views/content-management/faculties"),
            "bread-actions": () =>
                import(
                    "@/views/content-management/faculties/components/create-facultie.vue"
                )
        },
        meta: () => ({
            pageTitle: "الكليات",
            roles: [All],
            breadcrumb: [
                {
                    text: "الكليات",
                    active: true
                }
            ]
        })
    },
    {
        path: "/advertising",
        name: "advertising",
        components: {
            default: () => import("@/views/content-management/advertising"),
            "bread-actions": () =>
                import(
                    "@/views/content-management/advertising/components/button-create-advertising.vue"
                )
        },
        meta: () => ({
            pageTitle: "الإعلانات",
            roles: [All],
            breadcrumb: [
                {
                    text: "الإعلانات",
                    active: true
                }
            ]
        })
    },
    // --- contact
    {
        path: "/contact",
        name: "contact",
        components: {
            default: () => import("@/views/content-management/contact"),
            "bread-actions": () =>
                import(
                    "@/views/content-management/contact/components/create-contact.vue"
                )
        },
        meta: () => ({
            pageTitle: "راسلنا",
            roles: [All],
            breadcrumb: [
                {
                    text: "الرسائل المستلمة",
                    active: true
                }
            ]
        })
    },
    {
        path: "/contact/:id",
        name: "contact details",
        components: {
            default: () =>
                import("@/views/content-management/contact/pages/details.vue")
        },
        props: {
            default: true
        },
        meta: () => ({
            pageTitle: "راسلنا",
            roles: [All],
            breadcrumb: [
                {
                    text: "الرسائل المستلمة",

                    active: false,
                    to: "/contact"
                },
                {
                    text: "تفاصيل",
                    active: true
                }
            ]
        })
    },
    // --- subjects
    {
        path: "/subjects",
        name: "subjects",
        components: {
            default: () => import("@/views/content-management/subjects"),
            'bread-actions' : () => import("@/views/content-management/subjects/components/create-subject.vue"),
        },
        meta: () => ({
            pageTitle: "المواد",
            roles: [All],
            breadcrumb: [
                {
                    text: "المواد",
                    active: true,
                },
            ]
        }),
    },
    {
        path: "/subjects/:id",
        name: "subjects details",
        components: {
            default: () => import("@/views/content-management/subjects/pages/details.vue")
        },
        props: {
            default: true
        },
        meta: () => ({
            pageTitle: "المواد",
            roles: [All],
            breadcrumb: [
                {
                    text: "المواد",
                    active: false,
                    to:"/subjects"
                },
                {
                    text: "تفاصيل",
                    active: true,
                },
            ]
        }),
    },
    // ---courses
    {
        path: "/courses",
        name: "courses",
        components: {
            default: () => import("@/views/content-management/courses"),
            'bread-actions' : () => import("@/views/content-management/courses/components/create-courses.vue"),
        },
        meta: () => ({
            pageTitle: "الدورات",
            roles: [All],
            breadcrumb: [
                {
                    text: "الدورات",
                    active: true,
                },
            ]
        }),
    },
    {
        path: "/courses/:id",
        name: "courses details",
        components: {
            default: () => import("@/views/content-management/courses/pages/details.vue"),
            'bread-actions' : () => import("@/views/content-management/courses/components/create-courses.vue"),
        },
        props: {
            default: true,
            'bread-actions': true
        },
        meta: () => ({
            pageTitle: "الدورات",
            roles: [All],
            breadcrumb: [
                {
                    text: "الدورات",
                    active: false,
                    to:"/courses"
                },
                {
                    text: "تفاصيل",
                    active: true,
                },
            ]
        }),
    },
    // --- banks
    {
        path: "/banks",
        name: "banks",
        components: {
            default: () => import("@/views/content-management/banks"),
            'bread-actions' : () => import("@/views/content-management/banks/components/create-banks.vue"),
        },
        meta: () => ({
            pageTitle: "البنوك",
            roles: [All],
            breadcrumb: [
                {
                    text: "البنوك",
                    active: true,
                },
            ]
        }),
    },
    {
        path: "/banks/:id",
        name: "banks details",
        components: {
            default: () => import("@/views/content-management/banks/pages/details.vue"),
            'bread-actions' : () => import("@/views/content-management/banks/components/create-banks.vue"),
        },
        props: {
            default: true,
            'bread-actions': true
        },
        meta: () => ({
            pageTitle: "البنوك",
            roles: [All],
            breadcrumb: [
                {
                    text: "البنوك",
                    active: false,
                    to:"/banks"
                },
                {
                    text: "تفاصيل",
                    active: true,
                },
            ]
        }),
    },
    // ---telescope
    {
        path: "/telescope",
        name: "telescope",
        components: {
            default: () => import("@/views/content-management/telescope"),
            'bread-actions' : () => import("@/views/content-management/telescope/components/create-telescope.vue"),
        },
        meta: () => ({
            pageTitle: "المجاهر",
            roles: [All],
            breadcrumb: [
                {
                    text: "المجاهر",
                    active: true,
                },
            ]
        }),
    },
    {
        path: "/telescope/:id",
        name: "telescope details",
        components: {
            default: () => import("@/views/content-management/telescope/pages/details.vue")
        },
        props: {
            default: true
        },
        meta: () => ({
            pageTitle: "المجاهر",
            roles: [All],
            breadcrumb: [
                {
                    text: "المجاهر",
                    active: false,
                    to:"/telescope"
                },
                {
                    text: "تفاصيل",
                    active: true,
                },
            ]
        }),
    },
    // ---interviews
    {
        path: "/interviews",
        name: "interviews",
        components: {
            default: () => import("@/views/content-management/interviews"),
            'bread-actions' : () => import("@/views/content-management/interviews/components/create-interviews.vue"),
        },
        meta: () => ({
            pageTitle: "الأسئلة الكتابية",
            roles: [All],
            breadcrumb: [
                {
                    text: "الأسئلة الكتابية",
                    active: true,
                },
            ]
        }),
    },
    {
        path: "/interviews/:id",
        name: "interviews details",
        components: {
            default: () => import("@/views/content-management/interviews/pages/details.vue"),
            'bread-actions' : () => import("@/views/content-management/interviews/components/create-interviews.vue"),
        },
        props: {
            default: true,
            'bread-actions': true
        },
        meta: () => ({
            pageTitle: "الأسئلة الكتابية",
            roles: [All],
            breadcrumb: [
                {
                    text: "الأسئلة الكتابية",
                    active: false,
                    to:"/interviews"
                },
                {
                    text: "تفاصيل",
                    active: true,
                },
            ]
        }),
    },
    // --- notifications
    {
        path: "/notifications",
        name: "notifications",
        components: {
            default: () => import("@/views/content-management/notifications"),
            "bread-actions": () =>
                import(
                    "@/views/content-management/notifications/components/create-notifications.vue"
                )
        },
        meta: () => ({
            pageTitle: "الإشعارات",
            roles: [All],
            breadcrumb: [
                {
                    text: "الإشعارات",
                    active: true
                }
            ]
        })
    }
];
