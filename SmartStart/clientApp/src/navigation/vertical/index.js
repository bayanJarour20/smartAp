import { Admin } from '@/router'
export default [
  {
    title: 'الرئيسية',
    route: '/home',
    icon: 'home-alt',
    roles: [Admin]
  },
  {
    header: 'إدارة المحتوى',
    roles: [Admin]
  },
  {
    title: 'الكليات',
    route: '/faculties',
    icon: 'building',
    roles: [Admin] 
  },
  {
    title: 'المواد',
    route: '/subjects',
    icon: 'books',
    roles: [Admin]
  },
  {
    title: 'الأسئلة',
    route: '/questions',
    icon: 'file-question-alt',
    roles: [Admin]
  },
  {
    title: 'الدورات',
    route: '/courses',
    icon: 'file-alt',
    roles: [Admin]
  },
  {
    title: 'البنوك',
    route: '/banks',
    icon: 'bag-alt',
    roles: [Admin]
  },
  {
    title: 'المجاهر',
    route: '/telescope',
    icon: 'telescope',
    roles: [Admin]
  },
  {
    title: 'الأسئلة الكتابية',
    route: '/interviews',
    icon: 'chat-bubble-user',
    roles: [Admin]
  },
  {
    title: 'الإشعارات',
    route: '/notifications',
    icon: 'bell',
    roles: [Admin]
  },
  {
    title: 'راسلنا',
    route: '/contact',
    icon: 'envelope-alt',
    roles: [Admin]
  },
  {
      
    title: 'الإعلانات',
    route: '/advertising',
    icon: 'desktop',
    roles: [Admin]
  },
  {
    header: 'الأكواد',
    roles: [Admin]
  },
  {
    title: 'رموز التفعيل',
    route: '/codes',
    icon: 'qrcode-scan',
    roles: [Admin]
  },
  {
    title: 'كشف حساب',
    route: '/invoice',
    icon: 'bill',
    roles: [Admin]
  },
  {
    header: 'الحسابات',
    roles: [Admin]
  },
  {
    title: 'مستخدمو التطبيق',
    route: '/users/0',
    icon: 'users-alt',
    roles: [Admin]
  },
  {
    title: 'نقاط البيع',
    route: '/users/1',
    icon: 'money-withdrawal',
    roles: [Admin]
  },
  {
    header: 'الإعدادات العامة',
    roles: [Admin]
  },
  {
    title: 'مستخدمو اللوحة',
    route: '/users/2',
    icon: 'user-square',
    roles: [Admin]
  },
  {
    title: 'الإعدادات',
    route: '/settings',
    icon: 'cog',
    roles: [Admin]
  }
]
