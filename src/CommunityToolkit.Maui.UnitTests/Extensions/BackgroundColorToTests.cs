﻿using CommunityToolkit.Maui.Extensions;
using CommunityToolkit.Maui.UnitTests.Mocks;
using Xunit;

namespace CommunityToolkit.Maui.UnitTests.Extensions;

public class BackgroundColorToTests : BaseTest
{
	[Fact(Timeout = (int)TestDuration.Short)]
	public async Task BackgroundColorTo_VerifyColorChanged()
	{
		Color originalBackgroundColor = Colors.Blue, updatedBackgroundColor = Colors.Red;

		VisualElement element = new Label { BackgroundColor = originalBackgroundColor };
		element.EnableAnimations();

		Assert.Equal(originalBackgroundColor, element.BackgroundColor);

		var isSuccessful = await element.BackgroundColorTo(updatedBackgroundColor, token: TestContext.Current.CancellationToken);

		Assert.True(isSuccessful);
		Assert.Equal(updatedBackgroundColor, element.BackgroundColor);
	}

	[Fact(Timeout = (int)TestDuration.Medium)]
	public async Task BackgroundColorTo_CancellationTokenExpired()
	{
		var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1));

		VisualElement element = new Label { BackgroundColor = Colors.Blue };
		element.EnableAnimations();

		// Ensure CancellationToken has expired
		await Task.Delay(100, TestContext.Current.CancellationToken);

		await Assert.ThrowsAsync<OperationCanceledException>(() => element.BackgroundColorTo(Colors.Green, token: cts.Token));
	}

	[Fact(Timeout = (int)TestDuration.Short)]
	public async Task BackgroundColorTo_CancellationTokenCanceled()
	{
		var cts = new CancellationTokenSource();

		VisualElement element = new Label { BackgroundColor = Colors.Blue };
		element.EnableAnimations();

		// Ensure CancellationToken has expired
		await cts.CancelAsync();

		await Assert.ThrowsAsync<OperationCanceledException>(() => element.BackgroundColorTo(Colors.Green, token: cts.Token));
	}

	[Fact(Timeout = (int)TestDuration.Short)]
	public async Task BackgroundColorTo_VerifyColorChangedForDefaultBackgroundColor()
	{
		Color updatedBackgroundColor = Colors.Yellow;

		VisualElement element = new Label();
		element.EnableAnimations();

		var isSuccessful = await element.BackgroundColorTo(updatedBackgroundColor, token: TestContext.Current.CancellationToken);

		Assert.True(isSuccessful);
		Assert.Equal(updatedBackgroundColor, element.BackgroundColor);
	}

	[Fact(Timeout = (int)TestDuration.Short)]
	public async Task BackgroundColorTo_DoesNotAllowNullVisualElement()
	{
		VisualElement? element = null;

#pragma warning disable CS8603 // Possible null reference return.
		await Assert.ThrowsAsync<NullReferenceException>(() => element?.BackgroundColorTo(Colors.Red, token: TestContext.Current.CancellationToken));
#pragma warning restore CS8603 // Possible null reference return.
	}

	[Fact(Timeout = (int)TestDuration.Short)]
	public async Task BackgroundColorTo_DoesNotAllowNullColor()
	{
		VisualElement element = new Label();
		element.EnableAnimations();

#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
		await Assert.ThrowsAsync<ArgumentNullException>(() => element.BackgroundColorTo(null, token: TestContext.Current.CancellationToken));
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
	}
}