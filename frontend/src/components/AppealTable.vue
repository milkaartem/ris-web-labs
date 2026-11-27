<script setup>
defineProps({
  items: {
    type: Array,
    required: true,
  },
})
</script>

<template>
  <div class="table-wrap">
    <table class="appeals-table">
      <thead>
        <tr>
          <th>Номер</th>
          <th>Тема</th>
          <th>Услуга (serviceTypeId)</th>
          <th>Статус</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="item in items" :key="item.id">
          <td class="mono">{{ item.number }}</td>
          <td>{{ item.title }}</td>
          <td class="mono">{{ item.serviceTypeId }}</td>
          <td>
            <span class="status-badge" :data-status="item.status">
              {{ item.status }}
            </span>
          </td>
        </tr>
      </tbody>
    </table>
  </div>
</template>

<style scoped>
.table-wrap {
  overflow-x: auto;
}

.appeals-table {
  width: 100%;
  border-collapse: collapse;
  text-align: left;
}

.appeals-table th,
.appeals-table td {
  padding: 12px;
  border-bottom: 1px solid var(--border);
  vertical-align: middle;
}

.appeals-table th {
  font-size: 0.875rem;
  font-weight: 600;
  color: var(--muted);
  background-color: #f8fafc;
}

.mono {
  font-family: ui-monospace, Consolas, monospace;
  font-size: 0.9rem;
}

.status-badge {
  display: inline-block;
  padding: 3px 10px;
  border-radius: 999px;
  font-size: 0.8125rem;
  font-weight: 600;
}

.status-badge[data-status='New'] {
  background-color: #dbeafe;
  color: #1e40af;
}

.status-badge[data-status='InProgress'] {
  background-color: #fef3c7;
  color: #92400e;
}

.status-badge[data-status='Closed'] {
  background-color: #dcfce7;
  color: #166534;
}

.status-badge[data-status='Cancelled'] {
  background-color: #fee2e2;
  color: #991b1b;
}
</style>
