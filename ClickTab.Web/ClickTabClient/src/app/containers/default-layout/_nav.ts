import { INavData } from "@eqproject/eqp-ui";

/**
 * Voci della sidebar
 */
export const navItems: INavData[] = [
  {
    name: 'Dashboard',
    url: '/dashboard',
    icon: 'fa fa-home'
  },
  {
    name: 'Anagrafica',
    url: 'anagrafica',
    icon: 'fa fa-user',
    children: [
      {
        name: 'Dipendenti',
        url: '/employees',
        icon: '#'
      },
      {
        name: 'Mansioni',
        url: '/tasks',
        icon: '#'
      },
      {
        name: 'Incarichi',
        url: '/assignments',
        icon: '#'
      },
    ]
  },
  {
    name: 'Test',
    url: 'test',
    icon: 'fa fa-pie-chart',
  },

];

  // {
  //   name: 'Pages',
  //   url: '/login',
  //   iconComponent: { name: 'cil-star' },
  //   children: [
  //     {
  //       name: 'Login',
  //       url: '/login'
  //     },
  //     {
  //       name: 'Register',
  //       url: '/register'
  //     },
  //     {
  //       name: 'Error 404',
  //       url: '/404'
  //     },
  //     {
  //       name: 'Error 500',
  //       url: '/500'
  //     }
  //   ]
  // },

