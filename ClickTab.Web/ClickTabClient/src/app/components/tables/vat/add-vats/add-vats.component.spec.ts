import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddVatsComponent } from './add-vats.component';

describe('AddVatsComponent', () => {
  let component: AddVatsComponent;
  let fixture: ComponentFixture<AddVatsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AddVatsComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(AddVatsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
