import { INavData } from "@eqproject/eqp-ui";

function abs(u: string | undefined): string | undefined {
  if (!u) return u;
  return u.startsWith('/') ? u : '/' + u;
}

// Menu di navigazione
export const navItems: INavData[] = [
  {
    name: 'Dashboard',
    url: abs('/dashboard'),
    icon: 'fa fa-home'
  },
  {
    name: 'Anagrafica',
    url: '/registry',            // il modulo principale
    icon: 'fa fa-user',
    children: [
      {
        name: 'Clienti',
        url: 'list-users'        // ❌ niente slash davanti → è relativo al path "registry"
      },
      {
        name: 'Ruoli',
        url: 'list-roles'
      },
      {
        name: 'Prodotti',
        url: 'list-products'
      }
    ]
  }
];