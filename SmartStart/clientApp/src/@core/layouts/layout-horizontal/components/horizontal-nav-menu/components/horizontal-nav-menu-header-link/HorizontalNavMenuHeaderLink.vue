<template>
  <li
    v-if="canViewHorizontalNavMenuHeaderLink(item)"
    class="nav-item"
    :class="{'sidebar-group-active active': isActive}"
  >
    <b-link
      class="nav-link"
      :to="{ name: item.route }"
    >
      <feather-icon
        size="14"
        :icon="item.icon"
      />
      <span>{{  item.title }}</span>
    </b-link>
  </li>
</template>

<script>
import { BLink } from 'bootstrap-vue'
import { useUtils as useAclUtils } from '@core/libs/acl'
import useHorizontalNavMenuHeaderLink from './useHorizontalNavMenuHeaderLink'
import mixinHorizontalNavMenuHeaderLink from './mixinHorizontalNavMenuHeaderLink'

export default {
  components: {
    BLink,
  },
  mixins: [mixinHorizontalNavMenuHeaderLink],
  props: {
    item: {
      type: Object,
      required: true,
    },
  },
  setup(props) {
    const { isActive, updateIsActive } = useHorizontalNavMenuHeaderLink(props.item)

    const { canViewHorizontalNavMenuHeaderLink } = useAclUtils()

    return {
      isActive,
      updateIsActive,

      // ACL
      canViewHorizontalNavMenuHeaderLink,

    }
  },
}
</script>
