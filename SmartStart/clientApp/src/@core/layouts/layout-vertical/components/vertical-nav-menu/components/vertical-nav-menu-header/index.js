import { useUtils as useAclUtils } from '@core/libs/acl'
// import { getUserData } from "@/auth/utils";

const { canViewVerticalNavMenuHeader } = useAclUtils()

export default {
  props: {
    item: {
      type: Object,
      required: true,
    },
  },
  render(h) {
    const span = h('span', {}, this.item.header)
    const icon = h('feather-icon', { props: { icon: 'MoreHorizontalIcon', size: '18' } })
    // if (canViewVerticalNavMenuHeader(this.item) && this.item.roles.indexOf(getUserData().role) != -1) {
    if (canViewVerticalNavMenuHeader(this.item)) {
      return h('li', { class: 'navigation-header text-truncate' }, [span, icon])
    }
    return h()
  },
}
