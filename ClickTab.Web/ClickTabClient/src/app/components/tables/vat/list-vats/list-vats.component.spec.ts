import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ListVatsComponent } from './list-vats.component';

describe('ListVatsComponent', () => {
  let component: ListVatsComponent;
  let fixture: ComponentFixture<ListVatsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ListVatsComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(ListVatsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
