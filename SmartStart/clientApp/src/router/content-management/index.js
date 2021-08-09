import { All } from "@/router";
// import store from "@/store";
export default [
    // --- faculties
    {
        path: "/faculties",
        name: "faculties",
        components: {
            default: () => import("@/views/content-management/faculties"),
            'bread-actions' : () => import("@/views/content-management/faculties/components/create-facultie.vue"),
        },
        meta: () => ({
            pageTitle: "الكليات",
            roles: [All],
            breadcrumb: [
                {
                    text: "الكليات",
                    active: true,
                },
            ]
        }),
    },
    {
        path: "/advertising",
        name: "advertising",
        components: {
            default: () => import("@/views/content-management/advertising"),
            'bread-actions' : () => import("@/views/content-management/advertising/components/button-create-advertising.vue"),
        },
        meta: () => ({
            pageTitle: "الإعلانات",
            roles: [All],
            breadcrumb: [
                {
                    text: "الإعلانات",
                    active: true,
                },
            ]
        }),
    },
    // ---contact
    {
        path: "/contact",
        name: "contact",
        components: {
            default: () => import("@/views/content-management/contact"),
            'bread-actions' : () => import("@/views/content-management/contact/components/create-contact.vue"),
        },
        meta: () => ({
            pageTitle: "راسلنا",
            roles: [All],
            breadcrumb: [
                {
                    text: "الرسائل المستلمة",
                    active: true,
                },
            ]
        }),
    },
    {
        path: "/contact/:id",
        name: "contact details",
        components: {
            default: () => import("@/views/content-management/contact/pages/details.vue")
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
                    to:"/contact"
                },
                {
                    text: "تفاصيل",
                    active: true,
                },
            ]
        }),
    },
     // --- subjects
     {
        path: "/subjects",
        name: "subjects",
        components: {
            default: () => import("@/views/content-management/subjects/index.vue"),
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
]