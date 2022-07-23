public static class SuperLerp {

	public static float Scale(float OldMin, float OldMax, float NewMin, float NewMax, float OldValue){
	
		float OldRange = (OldMax - OldMin);
		float NewRange = (NewMax - NewMin);
		float NewValue = (((OldValue - OldMin) * NewRange) / OldRange) + NewMin;
	
		return(NewValue);
	}
}
