import { ref } from 'vue'

export const items = ref([])

export function setItems(nextItems) {
  items.value = nextItems
}
