// Copyright (C) 2026 SharpEmu Emulator Project
// SPDX-License-Identifier: GPL-2.0-or-later

namespace SharpEmu.HLE;

/// <summary>
/// Optional scheduler capability: delivers a queued kernel exception
/// (sceKernelRaiseException) addressed to the calling thread, running its
/// handler on this host thread. Host-parked HLE waits (a non-cooperative
/// pthread_cond_wait, sceKernelWaitSema, or sceKernelWaitEqueue) call this
/// between wait slices: a thread parked there never reaches the
/// import-boundary safe point where queued exceptions are normally consumed,
/// so IL2CPP's stop-the-world suspension would deadlock waiting for its
/// acknowledgement.
/// </summary>
public interface IGuestExceptionDeliveryScheduler
{
    /// <summary>
    /// Returns true when a handler ran. The caller must not hold the wait
    /// primitive's lock, since the handler may block on other primitives
    /// (e.g. the resume semaphore) while other threads signal this one.
    /// </summary>
    bool TryDeliverPendingGuestExceptionForCurrentThread(CpuContext callerContext);
}
